/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Query.Result;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Types;
using Sharpen.Util;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <summary>QQuery is the users hook on our graph.</summary>
	/// <remarks>
	/// QQuery is the users hook on our graph.
	/// A QQuery is defined by it's constraints.
	/// NOTE: This is just a 'partial' base class to allow for variant implementations
	/// in db4oj and db4ojdk1.2. It assumes that itself is an instance of QQuery
	/// and should never be used explicitly.
	/// </remarks>
	/// <exclude></exclude>
	public abstract class QQueryBase : IInternalQuery, IUnversioned
	{
		[System.NonSerialized]
		internal Db4objects.Db4o.Internal.Transaction _trans;

		private Collection4 i_constraints = new Collection4();

		private QQuery i_parent;

		private string i_field;

		[System.NonSerialized]
		private QueryEvaluationMode _evaluationMode;

		private int _prefetchDepth;

		private int _prefetchCount;

		private int _evaluationModeAsInt;

		private IQueryComparator _comparator;

		[System.NonSerialized]
		private readonly QQuery _this;

		private IList _orderings;

		protected QQueryBase()
		{
			// C/S only
			_this = Cast(this);
		}

		protected QQueryBase(Db4objects.Db4o.Internal.Transaction a_trans, QQuery a_parent
			, string a_field)
		{
			_this = Cast(this);
			_trans = a_trans;
			i_parent = a_parent;
			i_field = a_field;
		}

		public virtual void CaptureQueryResultConfig()
		{
			Config4Impl config = _trans.Container().Config();
			_evaluationMode = config.EvaluationMode();
			_prefetchDepth = config.PrefetchDepth();
			_prefetchCount = config.PrefetchObjectCount();
		}

		internal virtual void AddConstraint(QCon a_constraint)
		{
			i_constraints.Add(a_constraint);
		}

		private void AddConstraint(Collection4 col, object obj)
		{
			if (AttachToExistingConstraints(col, obj, true))
			{
				return;
			}
			if (AttachToExistingConstraints(col, obj, false))
			{
				return;
			}
			QConObject newConstraint = new QConObject(_trans, null, null, obj);
			AddConstraint(newConstraint);
			col.Add(newConstraint);
		}

		private bool AttachToExistingConstraints(Collection4 newConstraintsCollector, object
			 obj, bool onlyForPaths)
		{
			bool found = false;
			IEnumerator j = IterateConstraints();
			while (j.MoveNext())
			{
				QCon existingConstraint = (QCon)j.Current;
				BooleanByRef removeExisting = new BooleanByRef(false);
				if (!onlyForPaths || (existingConstraint is QConPath))
				{
					QCon newConstraint = existingConstraint.ShareParent(obj, removeExisting);
					if (newConstraint != null)
					{
						newConstraintsCollector.Add(newConstraint);
						AddConstraint(newConstraint);
						if (removeExisting.value)
						{
							RemoveConstraint(existingConstraint);
						}
						found = true;
						if (!onlyForPaths)
						{
							break;
						}
					}
				}
			}
			return found;
		}

		/// <summary>Search for slot that corresponds to class.</summary>
		/// <remarks>
		/// Search for slot that corresponds to class. <br />If not found add it.
		/// <br />Constrain it. <br />
		/// </remarks>
		public virtual IConstraint Constrain(object example)
		{
			lock (StreamLock())
			{
				IReflectClass claxx = ReflectClassForClass(example);
				if (claxx != null)
				{
					return AddClassConstraint(claxx);
				}
				QConEvaluation eval = Platform4.EvaluationCreate(_trans, example);
				if (eval != null)
				{
					return AddEvaluationToAllConstraints(eval);
				}
				Collection4 constraints = new Collection4();
				AddConstraint(constraints, example);
				return ToConstraint(constraints);
			}
		}

		private IConstraint AddEvaluationToAllConstraints(QConEvaluation eval)
		{
			if (i_constraints.Size() == 0)
			{
				_trans.Container().ClassCollection().IterateTopLevelClasses(new _IVisitor4_139(this
					));
			}
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				((QCon)i.Current).AddConstraint(eval);
			}
			// FIXME: should return valid Constraint object
			return null;
		}

		private sealed class _IVisitor4_139 : IVisitor4
		{
			public _IVisitor4_139(QQueryBase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object obj)
			{
				ClassMetadata classMetadata = (ClassMetadata)obj;
				QConClass qcc = new QConClass(this._enclosing._trans, classMetadata.ClassReflector
					());
				this._enclosing.AddConstraint(qcc);
				this._enclosing.ToConstraint(this._enclosing.i_constraints).Or(qcc);
			}

			private readonly QQueryBase _enclosing;
		}

		private IConstraint AddClassConstraint(IReflectClass claxx)
		{
			if (IsTheObjectClass(claxx))
			{
				return null;
			}
			if (claxx.IsInterface())
			{
				return AddInterfaceConstraint(claxx);
			}
			Collection4 newConstraints = IntroduceClassConstrain(claxx);
			if (newConstraints.IsEmpty())
			{
				QConClass qcc = new QConClass(_trans, claxx);
				AddConstraint(qcc);
				return qcc;
			}
			return ToConstraint(newConstraints);
		}

		private Collection4 IntroduceClassConstrain(IReflectClass claxx)
		{
			Collection4 newConstraints = new Collection4();
			IEnumerator existingConstraints = IterateConstraints();
			while (existingConstraints.MoveNext())
			{
				QCon existingConstraint = (QConObject)existingConstraints.Current;
				BooleanByRef removeExisting = new BooleanByRef(false);
				QCon newConstraint = existingConstraint.ShareParentForClass(claxx, removeExisting
					);
				if (newConstraint != null)
				{
					newConstraints.Add(newConstraint);
					AddConstraint(newConstraint);
					if (removeExisting.value)
					{
						RemoveConstraint(existingConstraint);
					}
				}
			}
			return newConstraints;
		}

		private bool IsTheObjectClass(IReflectClass claxx)
		{
			return claxx.Equals(Stream()._handlers.IclassObject);
		}

		private IConstraint AddInterfaceConstraint(IReflectClass claxx)
		{
			Collection4 classes = Stream().ClassCollection().ForInterface(claxx);
			if (classes.Size() == 0)
			{
				QConClass qcc = new QConClass(_trans, null, null, claxx);
				AddConstraint(qcc);
				return qcc;
			}
			IEnumerator i = classes.GetEnumerator();
			IConstraint constr = null;
			while (i.MoveNext())
			{
				ClassMetadata classMetadata = (ClassMetadata)i.Current;
				IReflectClass classMetadataClaxx = classMetadata.ClassReflector();
				if (classMetadataClaxx != null)
				{
					if (!classMetadataClaxx.IsInterface())
					{
						if (constr == null)
						{
							constr = Constrain(classMetadataClaxx);
						}
						else
						{
							constr = constr.Or(Constrain(classMetadata.ClassReflector()));
						}
					}
				}
			}
			return constr;
		}

		private IReflectClass ReflectClassForClass(object example)
		{
			if (example is IReflectClass)
			{
				return (IReflectClass)example;
			}
			if (example is Type)
			{
				return _trans.Reflector().ForClass((Type)example);
			}
			return null;
		}

		public virtual IConstraints Constraints()
		{
			lock (StreamLock())
			{
				IConstraint[] constraints = new IConstraint[i_constraints.Size()];
				i_constraints.ToArray(constraints);
				return new QConstraints(_trans, constraints);
			}
		}

		public virtual IQuery Descend(string a_field)
		{
			lock (StreamLock())
			{
				QQuery query = new QQuery(_trans, _this, a_field);
				IntByRef run = new IntByRef(1);
				if (!Descend1(query, a_field, run))
				{
					// try to add unparented nodes on the second run,
					// if not added in the first run and a descendant
					// was not found
					if (run.value == 1)
					{
						run.value = 2;
						if (!Descend1(query, a_field, run))
						{
							new QConUnconditional(_trans, false).Attach(query, a_field);
						}
					}
				}
				return query;
			}
		}

		private bool Descend1(QQuery query, string fieldName, IntByRef run)
		{
			if (run.value == 2 || i_constraints.Size() == 0)
			{
				// On the second run we are really creating a second independant
				// query network that is not joined to other higher level
				// constraints.
				// Let's see how this works out. We may need to join networks.
				run.value = 0;
				// prevent a double run of this code
				Stream().ClassCollection().AttachQueryNode(fieldName, new _IVisitor4_275(this));
			}
			CheckConstraintsEvaluationMode();
			BooleanByRef foundClass = new BooleanByRef(false);
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				if (((QCon)i.Current).Attach(query, fieldName))
				{
					foundClass.value = true;
				}
			}
			return foundClass.value;
		}

		private sealed class _IVisitor4_275 : IVisitor4
		{
			public _IVisitor4_275(QQueryBase _enclosing)
			{
				this._enclosing = _enclosing;
				this.untypedFieldConstraintCollected = false;
			}

			internal bool untypedFieldConstraintCollected;

			public void Visit(object obj)
			{
				object[] pair = ((object[])obj);
				ClassMetadata containingClass = (ClassMetadata)pair[0];
				FieldMetadata field = (FieldMetadata)pair[1];
				if (this.IsTyped(field))
				{
					this.AddFieldConstraint(containingClass, field);
					return;
				}
				if (this.untypedFieldConstraintCollected)
				{
					return;
				}
				this.AddFieldConstraint(containingClass, field);
				this.untypedFieldConstraintCollected = true;
			}

			private bool IsTyped(FieldMetadata field)
			{
				return !Handlers4.IsUntyped(field.GetHandler());
			}

			private void AddFieldConstraint(ClassMetadata containingClass, FieldMetadata field
				)
			{
				QConClass qcc = new QConClass(this._enclosing._trans, null, field.QField(this._enclosing
					._trans), containingClass.ClassReflector());
				this._enclosing.AddConstraint(qcc);
				this._enclosing.ToConstraint(this._enclosing.i_constraints).Or(qcc);
			}

			private readonly QQueryBase _enclosing;
		}

		public virtual IObjectSet Execute()
		{
			lock (StreamLock())
			{
				return ((IObjectSet)TriggeringQueryEvents(new _IClosure4_331(this)));
			}
		}

		private sealed class _IClosure4_331 : IClosure4
		{
			public _IClosure4_331(QQueryBase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				return new ObjectSetFacade(this._enclosing.GetQueryResult());
			}

			private readonly QQueryBase _enclosing;
		}

		public virtual void ExecuteLocal(IdListQueryResult result)
		{
			CheckConstraintsEvaluationMode();
			QQueryBase.CreateCandidateCollectionResult r = CreateCandidateCollection();
			bool checkDuplicates = r.checkDuplicates;
			bool topLevel = r.topLevel;
			List4 candidateCollection = r.candidateCollection;
			if (candidateCollection != null)
			{
				Collection4 executionPath = topLevel ? null : FieldPathFromTop();
				IEnumerator i = new Iterator4Impl(candidateCollection);
				while (i.MoveNext())
				{
					((QCandidates)i.Current).Execute();
				}
				if (((List4)candidateCollection._next) != null)
				{
					checkDuplicates = true;
				}
				if (checkDuplicates)
				{
					result.CheckDuplicates();
				}
				i = new Iterator4Impl(candidateCollection);
				while (i.MoveNext())
				{
					QCandidates candidates = (QCandidates)i.Current;
					if (topLevel)
					{
						candidates.TraverseIds(result);
					}
					else
					{
						candidates.TraverseIds(new QQueryBase.AscendingQueryExecutor(_trans, result, executionPath
							));
					}
				}
			}
			Sort(result);
		}

		private void TriggerQueryOnFinished()
		{
			Stream().Callbacks().QueryOnFinished(_trans, Cast(this));
		}

		private void TriggerQueryOnStarted()
		{
			Stream().Callbacks().QueryOnStarted(_trans, Cast(this));
		}

		public virtual IEnumerator ExecuteLazy()
		{
			CheckConstraintsEvaluationMode();
			QQueryBase.CreateCandidateCollectionResult r = CreateCandidateCollection();
			Collection4 executionPath = ExecutionPath(r);
			IEnumerator candidateCollection = new Iterator4Impl(r.candidateCollection);
			MappingIterator executeCandidates = new _MappingIterator_397(executionPath, candidateCollection
				);
			CompositeIterator4 resultingIDs = new CompositeIterator4(executeCandidates);
			if (!r.checkDuplicates)
			{
				return resultingIDs;
			}
			return CheckDuplicates(resultingIDs);
		}

		private sealed class _MappingIterator_397 : MappingIterator
		{
			public _MappingIterator_397(Collection4 executionPath, IEnumerator baseArg1) : base
				(baseArg1)
			{
				this.executionPath = executionPath;
			}

			protected override object Map(object current)
			{
				return ((QCandidates)current).ExecuteLazy(executionPath);
			}

			private readonly Collection4 executionPath;
		}

		public virtual IQueryResult GetQueryResult()
		{
			lock (StreamLock())
			{
				if (i_constraints.Size() == 0)
				{
					return ExecuteAllObjectsQuery();
				}
				IQueryResult result = ExecuteClassOnlyQuery();
				if (result != null)
				{
					return result;
				}
				OptimizeJoins();
				return ExecuteQuery();
			}
		}

		protected IQueryResult ExecuteQuery()
		{
			return Stream().ExecuteQuery(_this);
		}

		private IQueryResult ExecuteAllObjectsQuery()
		{
			return Stream().QueryAllObjects(_trans);
		}

		protected virtual ObjectContainerBase Stream()
		{
			return _trans.Container();
		}

		public virtual IInternalObjectContainer Container
		{
			get
			{
				return Stream();
			}
		}

		private IQueryResult ExecuteClassOnlyQuery()
		{
			ClassMetadata clazz = SingleClassConstraint();
			if (null == clazz)
			{
				return null;
			}
			IQueryResult queryResult = Stream().ClassOnlyQuery(this, clazz);
			Sort(queryResult);
			return queryResult;
		}

		private ClassMetadata SingleClassConstraint()
		{
			if (RequiresSort())
			{
				return null;
			}
			QConClass clazzconstr = ClassConstraint();
			if (clazzconstr == null)
			{
				return null;
			}
			ClassMetadata clazz = clazzconstr._classMetadata;
			if (clazz == null)
			{
				return null;
			}
			if (clazzconstr.HasChildren() || clazz.IsArray())
			{
				return null;
			}
			return clazz;
		}

		private QConClass ClassConstraint()
		{
			if (i_constraints.Size() != 1)
			{
				return null;
			}
			IConstraint constr = SingleConstraint();
			if (constr.GetType() != typeof(QConClass))
			{
				return null;
			}
			return (QConClass)constr;
		}

		private IConstraint SingleConstraint()
		{
			return (IConstraint)i_constraints.SingleElement();
		}

		private sealed class AscendingQueryExecutor : IIntVisitor
		{
			private readonly ObjectContainerBase _container;

			private readonly IdListQueryResult _result;

			private readonly Collection4 _executionPath;

			private readonly Transaction _trans;

			public AscendingQueryExecutor(Transaction trans, IdListQueryResult result, Collection4
				 executionPath)
			{
				_trans = trans;
				_container = _trans.Container();
				_result = result;
				_executionPath = executionPath;
			}

			public void Visit(int id)
			{
				TreeInt ids = new TreeInt(id);
				ByRef idsNew = new ByRef();
				IEnumerator itPath = _executionPath.GetEnumerator();
				while (itPath.MoveNext())
				{
					idsNew.value = null;
					string fieldName = (string)(itPath.Current);
					ids.Traverse(new _IVisitor4_508(this, fieldName, idsNew));
					ids = (TreeInt)((TreeInt)idsNew.value);
					if (ids == null)
					{
						return;
					}
				}
				ids.Traverse(new _IVisitor4_529(this));
			}

			private sealed class _IVisitor4_508 : IVisitor4
			{
				public _IVisitor4_508(AscendingQueryExecutor _enclosing, string fieldName, ByRef 
					idsNew)
				{
					this._enclosing = _enclosing;
					this.fieldName = fieldName;
					this.idsNew = idsNew;
				}

				public void Visit(object treeInt)
				{
					int id = ((TreeInt)treeInt)._key;
					StatefulBuffer buffer = this._enclosing._container.ReadStatefulBufferById(this._enclosing
						._trans, id);
					if (buffer != null)
					{
						ObjectHeader oh = new ObjectHeader(this._enclosing._container, buffer);
						CollectIdContext context = new CollectIdContext(this._enclosing._trans, oh, buffer
							);
						oh.ClassMetadata().CollectIDs(context, fieldName);
						Tree.Traverse(context.Ids(), new _IVisitor4_516(idsNew));
					}
				}

				private sealed class _IVisitor4_516 : IVisitor4
				{
					public _IVisitor4_516(ByRef idsNew)
					{
						this.idsNew = idsNew;
					}

					public void Visit(object node)
					{
						idsNew.value = TreeInt.Add(((TreeInt)idsNew.value), ((TreeInt)node)._key);
					}

					private readonly ByRef idsNew;
				}

				private readonly AscendingQueryExecutor _enclosing;

				private readonly string fieldName;

				private readonly ByRef idsNew;
			}

			private sealed class _IVisitor4_529 : IVisitor4
			{
				public _IVisitor4_529(AscendingQueryExecutor _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public void Visit(object treeInt)
				{
					this._enclosing._result.AddKeyCheckDuplicates(((TreeInt)treeInt)._key);
				}

				private readonly AscendingQueryExecutor _enclosing;
			}
		}

		public class CreateCandidateCollectionResult
		{
			public readonly bool checkDuplicates;

			public readonly bool topLevel;

			public readonly List4 candidateCollection;

			public CreateCandidateCollectionResult(List4 candidateCollection_, bool checkDuplicates_
				, bool topLevel_)
			{
				candidateCollection = candidateCollection_;
				topLevel = topLevel_;
				checkDuplicates = checkDuplicates_;
			}
		}

		public virtual IEnumerator ExecuteSnapshot()
		{
			QQueryBase.CreateCandidateCollectionResult r = CreateCandidateCollection();
			Collection4 executionPath = ExecutionPath(r);
			IEnumerator candidatesIterator = new Iterator4Impl(r.candidateCollection);
			Collection4 snapshots = new Collection4();
			while (candidatesIterator.MoveNext())
			{
				QCandidates candidates = (QCandidates)candidatesIterator.Current;
				snapshots.Add(candidates.ExecuteSnapshot(executionPath));
			}
			IEnumerator snapshotsIterator = snapshots.GetEnumerator();
			CompositeIterator4 resultingIDs = new CompositeIterator4(snapshotsIterator);
			if (!r.checkDuplicates)
			{
				return resultingIDs;
			}
			return CheckDuplicates(resultingIDs);
		}

		public virtual object TriggeringQueryEvents(IClosure4 closure)
		{
			TriggerQueryOnStarted();
			try
			{
				return closure.Run();
			}
			finally
			{
				TriggerQueryOnFinished();
			}
		}

		private IEnumerator CheckDuplicates(CompositeIterator4 executeAllCandidates)
		{
			return Iterators.Filter(executeAllCandidates, new _IPredicate4_582());
		}

		private sealed class _IPredicate4_582 : IPredicate4
		{
			public _IPredicate4_582()
			{
				this.ids = new TreeInt(0);
			}

			private TreeInt ids;

			public bool Match(object current)
			{
				int id = ((int)current);
				if (this.ids.Find(id) != null)
				{
					return false;
				}
				this.ids = (TreeInt)((TreeInt)this.ids.Add(new TreeInt(id)));
				return true;
			}
		}

		private Collection4 ExecutionPath(QQueryBase.CreateCandidateCollectionResult r)
		{
			return r.topLevel ? null : FieldPathFromTop();
		}

		public virtual void CheckConstraintsEvaluationMode()
		{
			IEnumerator constraints = IterateConstraints();
			while (constraints.MoveNext())
			{
				((QConObject)constraints.Current).SetEvaluationMode();
			}
		}

		private Collection4 FieldPathFromTop()
		{
			QQueryBase q = this;
			Collection4 fieldPath = new Collection4();
			while (q.i_parent != null)
			{
				fieldPath.Prepend(q.i_field);
				q = q.i_parent;
			}
			return fieldPath;
		}

		private void LogConstraints()
		{
		}

		public virtual QQueryBase.CreateCandidateCollectionResult CreateCandidateCollection
			()
		{
			List4 candidatesList = CreateQCandidatesList();
			bool checkDuplicates = false;
			bool topLevel = true;
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				QCon constraint = (QCon)i.Current;
				QCon old = constraint;
				constraint = constraint.GetRoot();
				if (constraint != old)
				{
					checkDuplicates = true;
					topLevel = false;
				}
				ClassMetadata classMetadata = constraint.GetYapClass();
				if (classMetadata == null)
				{
					break;
				}
				AddConstraintToCandidatesList(candidatesList, constraint);
			}
			return new QQueryBase.CreateCandidateCollectionResult(candidatesList, checkDuplicates
				, topLevel);
		}

		private void AddConstraintToCandidatesList(List4 candidatesList, QCon qcon)
		{
			if (candidatesList == null)
			{
				return;
			}
			IEnumerator j = new Iterator4Impl(candidatesList);
			while (j.MoveNext())
			{
				QCandidates candidates = (QCandidates)j.Current;
				candidates.AddConstraint(qcon);
			}
		}

		private List4 CreateQCandidatesList()
		{
			List4 candidatesList = null;
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				QCon constraint = (QCon)i.Current;
				constraint = constraint.GetRoot();
				ClassMetadata classMetadata = constraint.GetYapClass();
				if (classMetadata == null)
				{
					continue;
				}
				if (ConstraintCanBeAddedToExisting(candidatesList, constraint))
				{
					continue;
				}
				QCandidates candidates = new QCandidates((LocalTransaction)_trans, classMetadata, 
					null, true);
				candidatesList = new List4(candidatesList, candidates);
			}
			return candidatesList;
		}

		private bool ConstraintCanBeAddedToExisting(List4 candidatesList, QCon constraint
			)
		{
			IEnumerator j = new Iterator4Impl(candidatesList);
			while (j.MoveNext())
			{
				QCandidates candidates = (QCandidates)j.Current;
				if (candidates.FitsIntoExistingConstraintHierarchy(constraint))
				{
					return true;
				}
			}
			return false;
		}

		public Transaction Transaction()
		{
			return _trans;
		}

		public virtual IEnumerator IterateConstraints()
		{
			// clone the collection first to avoid
			// InvalidIteratorException as i_constraints might be 
			// modified during the execution of callee
			return new Collection4(i_constraints).GetEnumerator();
		}

		public virtual IQuery OrderAscending()
		{
			if (i_parent == null)
			{
				throw new InvalidOperationException("Cannot apply ordering at top level.");
			}
			lock (StreamLock())
			{
				AddOrdering(SodaQueryComparator.Direction.Ascending);
				return _this;
			}
		}

		public virtual IQuery OrderDescending()
		{
			if (i_parent == null)
			{
				throw new InvalidOperationException("Cannot apply ordering at top level.");
			}
			lock (StreamLock())
			{
				AddOrdering(SodaQueryComparator.Direction.Descending);
				return _this;
			}
		}

		private void AddOrdering(SodaQueryComparator.Direction direction)
		{
			AddOrdering(direction, new ArrayList());
		}

		protected void AddOrdering(SodaQueryComparator.Direction direction, IList path)
		{
			if (i_field != null)
			{
				path.Add(i_field);
			}
			if (i_parent != null)
			{
				i_parent.AddOrdering(direction, path);
				return;
			}
			string[] fieldPath = ReverseFieldPath(path);
			RemoveExistingOrderingFor(fieldPath);
			Orderings().Add(new SodaQueryComparator.Ordering(direction, fieldPath));
		}

		private void RemoveExistingOrderingFor(string[] fieldPath)
		{
			for (IEnumerator orderingIter = Orderings().GetEnumerator(); orderingIter.MoveNext
				(); )
			{
				SodaQueryComparator.Ordering ordering = ((SodaQueryComparator.Ordering)orderingIter
					.Current);
				if (Arrays.Equals(ordering.FieldPath(), fieldPath))
				{
					Orderings().Remove(ordering);
					break;
				}
			}
		}

		/// <summary>Public so it can be used by the LINQ test cases.</summary>
		/// <remarks>Public so it can be used by the LINQ test cases.</remarks>
		public IList Orderings()
		{
			if (null == _orderings)
			{
				_orderings = new ArrayList();
			}
			return _orderings;
		}

		private string[] ReverseFieldPath(IList path)
		{
			string[] reversedPath = new string[path.Count];
			for (int i = 0; i < reversedPath.Length; i++)
			{
				reversedPath[i] = ((string)path[path.Count - i - 1]);
			}
			return reversedPath;
		}

		public virtual void Marshall()
		{
			CheckConstraintsEvaluationMode();
			_evaluationModeAsInt = _evaluationMode.AsInt();
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				((QCon)i.Current).GetRoot().Marshall();
			}
		}

		public virtual void Unmarshall(Transaction a_trans)
		{
			_evaluationMode = QueryEvaluationMode.FromInt(_evaluationModeAsInt);
			_trans = a_trans;
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				((QCon)i.Current).Unmarshall(a_trans);
			}
		}

		internal virtual void RemoveConstraint(QCon a_constraint)
		{
			i_constraints.Remove(a_constraint);
		}

		internal virtual IConstraint ToConstraint(Collection4 constraints)
		{
			if (constraints.Size() == 1)
			{
				return (IConstraint)constraints.SingleElement();
			}
			else
			{
				if (constraints.Size() > 0)
				{
					IConstraint[] constraintArray = new IConstraint[constraints.Size()];
					constraints.ToArray(constraintArray);
					return new QConstraints(_trans, constraintArray);
				}
			}
			return null;
		}

		protected virtual object StreamLock()
		{
			return Stream().Lock();
		}

		public virtual IQuery SortBy(IQueryComparator comparator)
		{
			_comparator = comparator;
			return _this;
		}

		private void Sort(IQueryResult result)
		{
			if (_orderings != null)
			{
				result.SortIds(NewSodaQueryComparator());
			}
			if (_comparator != null)
			{
				result.Sort(_comparator);
			}
		}

		private IIntComparator NewSodaQueryComparator()
		{
			return new SodaQueryComparator((LocalObjectContainer)this.Transaction().Container
				(), ExtentType(), ((SodaQueryComparator.Ordering[])Sharpen.Collections.ToArray(_orderings
				, new SodaQueryComparator.Ordering[_orderings.Count])));
		}

		private ClassMetadata ExtentType()
		{
			return ClassConstraint().GetYapClass();
		}

		// cheat emulating '(QQuery)this'
		private static QQuery Cast(QQueryBase obj)
		{
			return (QQuery)obj;
		}

		public virtual bool RequiresSort()
		{
			if (_comparator != null || _orderings != null)
			{
				return true;
			}
			return false;
		}

		public virtual IQueryComparator Comparator()
		{
			return _comparator;
		}

		public virtual QueryEvaluationMode EvaluationMode()
		{
			return _evaluationMode;
		}

		public virtual void EvaluationMode(QueryEvaluationMode mode)
		{
			_evaluationMode = mode;
		}

		private void OptimizeJoins()
		{
			if (!HasOrJoins())
			{
				RemoveJoins();
			}
		}

		private bool HasOrJoins()
		{
			return ForEachConstraintRecursively(new _IFunction4_864());
		}

		private sealed class _IFunction4_864 : IFunction4
		{
			public _IFunction4_864()
			{
			}

			public object Apply(object obj)
			{
				QCon constr = (QCon)obj;
				IEnumerator joinIter = constr.IterateJoins();
				while (joinIter.MoveNext())
				{
					QConJoin join = (QConJoin)joinIter.Current;
					if (join.IsOr())
					{
						return true;
					}
				}
				return false;
			}
		}

		private void RemoveJoins()
		{
			ForEachConstraintRecursively(new _IFunction4_880());
		}

		private sealed class _IFunction4_880 : IFunction4
		{
			public _IFunction4_880()
			{
			}

			public object Apply(object obj)
			{
				QCon constr = (QCon)obj;
				constr.i_joins = null;
				return false;
			}
		}

		private bool ForEachConstraintRecursively(IFunction4 block)
		{
			IQueue4 queue = new NoDuplicatesQueue(new NonblockingQueue());
			IEnumerator constrIter = IterateConstraints();
			while (constrIter.MoveNext())
			{
				queue.Add(constrIter.Current);
			}
			while (queue.HasNext())
			{
				QCon constr = (QCon)queue.Next();
				bool cancel = (bool)block.Apply(constr);
				if (cancel)
				{
					return true;
				}
				IEnumerator childIter = constr.IterateChildren();
				while (childIter.MoveNext())
				{
					queue.Add(childIter.Current);
				}
				IEnumerator joinIter = constr.IterateJoins();
				while (joinIter.MoveNext())
				{
					queue.Add(joinIter.Current);
				}
			}
			return false;
		}

		public virtual int PrefetchDepth()
		{
			return _prefetchDepth;
		}

		public virtual int PrefetchCount()
		{
			return _prefetchCount;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("QQueryBase\n");
			IEnumerator i = IterateConstraints();
			while (i.MoveNext())
			{
				QCon constraint = (QCon)i.Current;
				sb.Append(constraint);
				sb.Append("\n");
			}
			return sb.ToString();
		}

		public virtual QQuery Parent()
		{
			return i_parent;
		}
	}
}
