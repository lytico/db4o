/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.Diagnostic;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public sealed class QCandidates : IFieldFilterable
	{
		public readonly LocalTransaction i_trans;

		public QueryResultCandidates _result;

		private List4 _constraints;

		internal ClassMetadata _classMetadata;

		private QField _field;

		internal QCon _currentConstraint;

		private IDGenerator _idGenerator;

		private bool _loadedFromClassIndex;

		private bool _loadedFromClassFieldIndex;

		private bool _isTopLevel;

		internal QCandidates(LocalTransaction a_trans, ClassMetadata a_classMetadata, QField
			 a_field, bool isTopLevel)
		{
			// Transaction necessary as reference to stream
			// collection of all constraints
			// possible class information
			// possible field information
			// current executing constraint, only set where needed
			_result = new QueryResultCandidates(this);
			_isTopLevel = isTopLevel;
			i_trans = a_trans;
			_classMetadata = a_classMetadata;
			_field = a_field;
			if (a_field == null || a_field._fieldMetadata == null || !(a_field._fieldMetadata
				.GetHandler() is StandardReferenceTypeHandler))
			{
				return;
			}
			ClassMetadata yc = ((StandardReferenceTypeHandler)a_field._fieldMetadata.GetHandler
				()).ClassMetadata();
			if (_classMetadata == null)
			{
				_classMetadata = yc;
			}
			else
			{
				yc = _classMetadata.GetHigherOrCommonHierarchy(yc);
				if (yc != null)
				{
					_classMetadata = yc;
				}
			}
		}

		public bool IsTopLevel()
		{
			return _isTopLevel;
		}

		public IInternalCandidate Add(IInternalCandidate candidate)
		{
			_result.Add(candidate);
			if (((QCandidateBase)candidate)._size == 0)
			{
				// This means that the candidate was already present
				// and QCandidate does not allow duplicates.
				// In this case QCandidate#isDuplicateOf will have
				// placed the existing QCandidate in the i_root
				// variable of the new candidate. We return it here: 
				return candidate.GetRoot();
			}
			return candidate;
		}

		internal void AddConstraint(QCon a_constraint)
		{
			_constraints = new List4(_constraints, a_constraint);
		}

		public IInternalCandidate ReadSubCandidate(QueryingReadContext context, ITypeHandler4
			 handler)
		{
			ObjectID objectID = ObjectID.NotPossible;
			try
			{
				int offset = context.Offset();
				if (handler is IReadsObjectIds)
				{
					objectID = ((IReadsObjectIds)handler).ReadObjectID(context);
				}
				if (objectID.IsValid())
				{
					return new QCandidate(this, null, objectID._id);
				}
				if (objectID == ObjectID.NotPossible)
				{
					context.Seek(offset);
					object obj = context.Read(handler);
					if (obj != null)
					{
						int id = context.Container().GetID(context.Transaction(), obj);
						if (id == 0)
						{
							return new QPrimitiveCandidate(this, obj);
						}
						QCandidate candidate = new QCandidate(this, obj, id);
						candidate.ClassMetadata(context.Container().ClassMetadataForObject(obj));
						return candidate;
					}
				}
			}
			catch (Exception)
			{
			}
			// FIXME: Catchall
			return null;
		}

		internal void Collect(Db4objects.Db4o.Internal.Query.Processor.QCandidates a_candidates
			)
		{
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				QCon qCon = (QCon)i.Current;
				SetCurrentConstraint(qCon);
				qCon.Collect(a_candidates);
			}
			SetCurrentConstraint(null);
		}

		internal void Execute()
		{
			if (DTrace.enabled)
			{
				DTrace.QueryProcess.Log();
			}
			FieldIndexProcessorResult result = ProcessFieldIndexes();
			if (result.FoundIndex())
			{
				_result.FieldIndexProcessorResult(result);
			}
			else
			{
				LoadFromClassIndex();
			}
			Evaluate();
		}

		public IEnumerator ExecuteSnapshot(Collection4 executionPath)
		{
			IIntIterator4 indexIterator = new IntIterator4Adaptor(IterateIndex(ProcessFieldIndexes
				()));
			Tree idRoot = TreeInt.AddAll(null, indexIterator);
			IEnumerator snapshotIterator = new TreeKeyIterator(idRoot);
			IEnumerator singleObjectQueryIterator = SingleObjectSodaProcessor(snapshotIterator
				);
			return MapIdsToExecutionPath(singleObjectQueryIterator, executionPath);
		}

		private IEnumerator SingleObjectSodaProcessor(IEnumerator indexIterator)
		{
			return Iterators.Map(indexIterator, new _IFunction4_171(this));
		}

		private sealed class _IFunction4_171 : IFunction4
		{
			public _IFunction4_171(QCandidates _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object current)
			{
				int id = ((int)current);
				QCandidateBase candidate = new QCandidate(this._enclosing, null, id);
				this._enclosing._result.SingleCandidate(candidate);
				this._enclosing.Evaluate();
				if (!candidate.Include())
				{
					return Iterators.Skip;
				}
				return current;
			}

			private readonly QCandidates _enclosing;
		}

		public IEnumerator ExecuteLazy(Collection4 executionPath)
		{
			IEnumerator indexIterator = IterateIndex(ProcessFieldIndexes());
			IEnumerator singleObjectQueryIterator = SingleObjectSodaProcessor(indexIterator);
			return MapIdsToExecutionPath(singleObjectQueryIterator, executionPath);
		}

		private IEnumerator IterateIndex(FieldIndexProcessorResult result)
		{
			if (result.NoMatch())
			{
				return Iterators.EmptyIterator;
			}
			if (result.FoundIndex())
			{
				return result.IterateIDs();
			}
			if (!_classMetadata.HasClassIndex())
			{
				return Iterators.EmptyIterator;
			}
			return BTreeClassIndexStrategy.Iterate(_classMetadata, i_trans);
		}

		private IEnumerator MapIdsToExecutionPath(IEnumerator singleObjectQueryIterator, 
			Collection4 executionPath)
		{
			if (executionPath == null)
			{
				return singleObjectQueryIterator;
			}
			IEnumerator res = singleObjectQueryIterator;
			IEnumerator executionPathIterator = executionPath.GetEnumerator();
			while (executionPathIterator.MoveNext())
			{
				string fieldName = (string)executionPathIterator.Current;
				res = Iterators.Concat(Iterators.Map(res, new _IFunction4_217(this, fieldName)));
			}
			return res;
		}

		private sealed class _IFunction4_217 : IFunction4
		{
			public _IFunction4_217(QCandidates _enclosing, string fieldName)
			{
				this._enclosing = _enclosing;
				this.fieldName = fieldName;
			}

			public object Apply(object current)
			{
				int id = ((int)current);
				CollectIdContext context = CollectIdContext.ForID(this._enclosing.i_trans, id);
				if (context == null)
				{
					return Iterators.Skip;
				}
				context.ClassMetadata().CollectIDs(context, fieldName);
				return new TreeKeyIterator(context.Ids());
			}

			private readonly QCandidates _enclosing;

			private readonly string fieldName;
		}

		public ObjectContainerBase Stream()
		{
			return i_trans.Container();
		}

		public int ClassIndexEntryCount()
		{
			return _classMetadata.IndexEntryCount(i_trans);
		}

		private FieldIndexProcessorResult ProcessFieldIndexes()
		{
			if (_constraints == null)
			{
				return FieldIndexProcessorResult.NoIndexFound;
			}
			return new FieldIndexProcessor(this).Run();
		}

		internal void Evaluate()
		{
			if (_constraints == null)
			{
				return;
			}
			ForEachConstraint(new _IProcedure4_254(this));
			ForEachConstraint(new _IProcedure4_262());
			ForEachConstraint(new _IProcedure4_268());
			ForEachConstraint(new _IProcedure4_274());
			ForEachConstraint(new _IProcedure4_280());
			ForEachConstraint(new _IProcedure4_286());
		}

		private sealed class _IProcedure4_254 : IProcedure4
		{
			public _IProcedure4_254(QCandidates _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Apply(object arg)
			{
				QCon qCon = (QCon)arg;
				qCon.SetCandidates(this._enclosing);
				qCon.EvaluateSelf();
			}

			private readonly QCandidates _enclosing;
		}

		private sealed class _IProcedure4_262 : IProcedure4
		{
			public _IProcedure4_262()
			{
			}

			public void Apply(object arg)
			{
				((QCon)arg).EvaluateSimpleChildren();
			}
		}

		private sealed class _IProcedure4_268 : IProcedure4
		{
			public _IProcedure4_268()
			{
			}

			public void Apply(object arg)
			{
				((QCon)arg).EvaluateEvaluations();
			}
		}

		private sealed class _IProcedure4_274 : IProcedure4
		{
			public _IProcedure4_274()
			{
			}

			public void Apply(object arg)
			{
				((QCon)arg).EvaluateCreateChildrenCandidates();
			}
		}

		private sealed class _IProcedure4_280 : IProcedure4
		{
			public _IProcedure4_280()
			{
			}

			public void Apply(object arg)
			{
				((QCon)arg).EvaluateCollectChildren();
			}
		}

		private sealed class _IProcedure4_286 : IProcedure4
		{
			public _IProcedure4_286()
			{
			}

			public void Apply(object arg)
			{
				((QCon)arg).EvaluateChildren();
			}
		}

		private void ForEachConstraint(IProcedure4 proc)
		{
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				QCon constraint = (QCon)i.Current;
				if (!constraint.ProcessedByIndex())
				{
					proc.Apply(constraint);
				}
			}
		}

		internal bool IsEmpty()
		{
			bool[] ret = new bool[] { true };
			Traverse(new _IVisitor4_306(ret));
			return ret[0];
		}

		private sealed class _IVisitor4_306 : IVisitor4
		{
			public _IVisitor4_306(bool[] ret)
			{
				this.ret = ret;
			}

			public void Visit(object obj)
			{
				if (((IInternalCandidate)obj).Include())
				{
					ret[0] = false;
				}
			}

			private readonly bool[] ret;
		}

		internal bool Filter(IVisitor4 visitor)
		{
			return _result.Filter(visitor);
		}

		internal bool Filter(QField field, IFieldFilterable filterable)
		{
			return _result.Filter(field, filterable);
		}

		internal int GenerateCandidateId()
		{
			if (_idGenerator == null)
			{
				_idGenerator = new IDGenerator();
			}
			return -_idGenerator.Next();
		}

		public IEnumerator IterateConstraints()
		{
			if (_constraints == null)
			{
				return Iterators.EmptyIterator;
			}
			return new Iterator4Impl(_constraints);
		}

		internal void LoadFromClassIndex()
		{
			if (!IsEmpty())
			{
				return;
			}
			_result.LoadFromClassIndex(_classMetadata.Index());
			DiagnosticProcessor dp = i_trans.Container()._handlers.DiagnosticProcessor();
			if (dp.Enabled() && !IsClassOnlyQuery())
			{
				dp.LoadedFromClassIndex(_classMetadata);
			}
			_loadedFromClassIndex = true;
		}

		internal void SetCurrentConstraint(QCon a_constraint)
		{
			_currentConstraint = a_constraint;
		}

		internal void Traverse(IVisitor4 visitor)
		{
			_result.Traverse(visitor);
		}

		internal void TraverseIds(IIntVisitor visitor)
		{
			_result.TraverseIds(visitor);
		}

		// FIXME: This method should go completely.
		//        We changed the code to create the QCandidates graph in two steps:
		//        (1) call fitsIntoExistingConstraintHierarchy to determine whether
		//            or not we need more QCandidates objects
		//        (2) add all constraints
		//        This method tries to do both in one, which results in missing
		//        constraints. Not all are added to all QCandiates.
		//        Right methodology is in 
		//        QQueryBase#createCandidateCollection
		//        and
		//        QQueryBase#createQCandidatesList
		internal bool TryAddConstraint(QCon a_constraint)
		{
			if (_field != null)
			{
				QField qf = a_constraint.GetField();
				if (qf != null)
				{
					if (_field.Name() != null && !_field.Name().Equals(qf.Name()))
					{
						return false;
					}
				}
			}
			if (_classMetadata == null || a_constraint.IsNullConstraint())
			{
				AddConstraint(a_constraint);
				return true;
			}
			ClassMetadata yc = a_constraint.GetYapClass();
			if (yc != null)
			{
				yc = _classMetadata.GetHigherOrCommonHierarchy(yc);
				if (yc != null)
				{
					_classMetadata = yc;
					AddConstraint(a_constraint);
					return true;
				}
			}
			AddConstraint(a_constraint);
			return false;
		}

		//    public void visit(Object a_tree) {
		//    	final QCandidate parent = (QCandidate) a_tree;
		//    	if (parent.createChild(this)) {
		//    		return;
		//    	}
		//    	
		//    	// No object found.
		//    	// All children constraints are necessarily false.
		//    	// Check immediately.
		//		Iterator4 i = iterateConstraints();
		//		while(i.moveNext()){
		//			((QCon)i.current()).visitOnNull(parent.getRoot());
		//		}
		//    		
		//    }
		public void Filter(QField field, IParentCandidate parent)
		{
			if (parent.CreateChild(field, this))
			{
				return;
			}
			// No object found.
			// All children constraints are necessarily false.
			// Check immediately.
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				((QCon)i.Current).VisitOnNull(parent.GetRoot());
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			_result.Traverse(new _IVisitor4_439(sb));
			return sb.ToString();
		}

		private sealed class _IVisitor4_439 : IVisitor4
		{
			public _IVisitor4_439(StringBuilder sb)
			{
				this.sb = sb;
			}

			public void Visit(object obj)
			{
				QCandidateBase candidate = (QCandidateBase)obj;
				sb.Append(" ");
				sb.Append(candidate._key);
			}

			private readonly StringBuilder sb;
		}

		public Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return i_trans;
		}

		public bool WasLoadedFromClassIndex()
		{
			return _loadedFromClassIndex;
		}

		public bool WasLoadedFromClassFieldIndex()
		{
			return _loadedFromClassFieldIndex;
		}

		public void WasLoadedFromClassFieldIndex(bool flag)
		{
			_loadedFromClassFieldIndex = flag;
		}

		public bool FitsIntoExistingConstraintHierarchy(QCon constraint)
		{
			if (_field != null)
			{
				QField qf = constraint.GetField();
				if (qf != null)
				{
					if (_field.Name() != null && !_field.Name().Equals(qf.Name()))
					{
						return false;
					}
				}
			}
			if (_classMetadata == null || constraint.IsNullConstraint())
			{
				return true;
			}
			ClassMetadata classMetadata = constraint.GetYapClass();
			if (classMetadata == null)
			{
				return false;
			}
			classMetadata = _classMetadata.GetHigherOrCommonHierarchy(classMetadata);
			if (classMetadata == null)
			{
				return false;
			}
			_classMetadata = classMetadata;
			return true;
		}

		private bool IsClassOnlyQuery()
		{
			if (((List4)_constraints._next) != null)
			{
				return false;
			}
			if (!(_constraints._element is QConClass))
			{
				return false;
			}
			return !((QCon)_constraints._element).HasChildren();
		}
	}
}
