/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <summary>Represents an actual object in the database.</summary>
	/// <remarks>
	/// Represents an actual object in the database. Forms a tree structure, indexed
	/// by id. Can have dependents that are doNotInclude'd in the query result when
	/// this is doNotInclude'd.
	/// </remarks>
	/// <exclude></exclude>
	public class QCandidate : QCandidateBase, IParentCandidate
	{
		internal ByteArrayBuffer _bytes;

		internal object _member;

		internal Db4objects.Db4o.Internal.ClassMetadata _classMetadata;

		internal FieldMetadata _fieldMetadata;

		private int _handlerVersion;

		public QCandidate(QCandidates candidates, object member, int id) : base(candidates
			, id)
		{
			// db4o ID is stored in _key;
			// db4o byte stream storing the object
			// the ClassMetadata of this object
			// temporary field and member for one field during evaluation
			// null denotes null object
			_member = member;
		}

		public override object ShallowClone()
		{
			Db4objects.Db4o.Internal.Query.Processor.QCandidate qcan = new Db4objects.Db4o.Internal.Query.Processor.QCandidate
				(_candidates, _member, _key);
			qcan.SetBytes(_bytes);
			qcan._dependants = _dependants;
			qcan._include = _include;
			qcan._member = _member;
			qcan._pendingJoins = _pendingJoins;
			qcan._root = _root;
			qcan._classMetadata = _classMetadata;
			qcan._fieldMetadata = _fieldMetadata;
			return base.ShallowCloneInternal(qcan);
		}

		private void CheckInstanceOfCompare()
		{
			if (_member is ICompare)
			{
				_member = ((ICompare)_member).Compare();
				LocalObjectContainer stream = Container();
				_classMetadata = stream.ClassMetadataForReflectClass(stream.Reflector().ForObject
					(_member));
				_key = stream.GetID(Transaction(), _member);
				if (_key == 0)
				{
					SetBytes(null);
				}
				else
				{
					SetBytes(stream.ReadBufferById(Transaction(), _key));
				}
			}
		}

		public virtual bool CreateChild(QField field, QCandidates candidates)
		{
			if (!_include)
			{
				return false;
			}
			UseField(field);
			if (_fieldMetadata == null || _fieldMetadata is NullFieldMetadata)
			{
				return false;
			}
			ITypeHandler4 handler = _fieldMetadata.GetHandler();
			if (handler != null)
			{
				QueryingReadContext queryingReadContext = new QueryingReadContext(Transaction(), 
					MarshallerFamily().HandlerVersion(), _bytes, _key);
				ITypeHandler4 arrayElementHandler = Handlers4.ArrayElementHandler(handler, queryingReadContext
					);
				if (arrayElementHandler != null)
				{
					return CreateChildForDescendable(candidates, handler, queryingReadContext, arrayElementHandler
						);
				}
				// We may get simple types here too, if the YapField was null
				// in the higher level simple evaluation. Evaluate these
				// immediately.
				if (Handlers4.IsQueryLeaf(handler))
				{
					candidates._currentConstraint.Visit(this);
					return true;
				}
			}
			_classMetadata.SeekToField(Transaction(), _bytes, _fieldMetadata);
			IInternalCandidate candidate = ReadSubCandidate(candidates);
			if (candidate == null)
			{
				return false;
			}
			// fast early check for ClassMetadata
			if (candidates._classMetadata != null && candidates._classMetadata.IsStronglyTyped
				())
			{
				if (Handlers4.IsUntyped(handler))
				{
					handler = TypeHandlerFor(candidate);
				}
				if (handler == null)
				{
					return false;
				}
			}
			AddDependant(candidates.Add(candidate));
			return true;
		}

		private bool CreateChildForDescendable(QCandidates parentCandidates, ITypeHandler4
			 handler, QueryingReadContext queryingReadContext, ITypeHandler4 arrayElementHandler
			)
		{
			int offset = queryingReadContext.Offset();
			bool outerRes = true;
			// The following construct is worse than not ideal. For each constraint it completely reads the
			// underlying structure again. The structure could be kept fairly easy. TODO: Optimize!
			IEnumerator i = parentCandidates.IterateConstraints();
			while (i.MoveNext())
			{
				QCon qcon = (QCon)i.Current;
				QField qf = qcon.GetField();
				if (qf != null && !qf.Name().Equals(_fieldMetadata.GetName()))
				{
					continue;
				}
				QCon tempParent = qcon.Parent();
				qcon.SetParent(null);
				QCandidates candidates = new QCandidates(parentCandidates.i_trans, null, qf, false
					);
				candidates.AddConstraint(qcon);
				qcon.SetCandidates(candidates);
				ReadArrayCandidates(handler, queryingReadContext.Buffer(), arrayElementHandler, candidates
					);
				queryingReadContext.Seek(offset);
				bool isNot = qcon.IsNot();
				if (isNot)
				{
					qcon.RemoveNot();
				}
				candidates.Evaluate();
				ByRef pending = ByRef.NewInstance();
				BooleanByRef innerRes = new BooleanByRef(isNot);
				candidates.Traverse(new QCandidate.CreateDescendChildTraversingVisitor(pending, innerRes
					, isNot));
				if (isNot)
				{
					qcon.Not();
				}
				// In case we had pending subresults, we need to communicate them up to our root.
				if (((Tree)pending.value) != null)
				{
					((Tree)pending.value).Traverse(new _IVisitor4_168(this));
				}
				if (!innerRes.value)
				{
					// Again this could be double triggering.
					// 
					// We want to clean up the "No route" at some stage.
					qcon.Visit(GetRoot(), qcon.Evaluator().Not(false));
					outerRes = false;
				}
				qcon.SetParent(tempParent);
			}
			return outerRes;
		}

		private sealed class _IVisitor4_168 : IVisitor4
		{
			public _IVisitor4_168(QCandidate _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object a_object)
			{
				this._enclosing.GetRoot().Evaluate((QPending)a_object);
			}

			private readonly QCandidate _enclosing;
		}

		private ITypeHandler4 TypeHandlerFor(IInternalCandidate candidate)
		{
			Db4objects.Db4o.Internal.ClassMetadata classMetadata = candidate.ClassMetadata();
			if (classMetadata != null)
			{
				return classMetadata.TypeHandler();
			}
			return null;
		}

		private void ReadArrayCandidates(ITypeHandler4 typeHandler, IReadBuffer buffer, ITypeHandler4
			 arrayElementHandler, QCandidates candidates)
		{
			if (!Handlers4.IsCascading(arrayElementHandler))
			{
				return;
			}
			SlotFormat slotFormat = SlotFormat.ForHandlerVersion(_handlerVersion);
			slotFormat.DoWithSlotIndirection(buffer, typeHandler, new _IClosure4_205(this, arrayElementHandler
				, buffer, candidates));
		}

		private sealed class _IClosure4_205 : IClosure4
		{
			public _IClosure4_205(QCandidate _enclosing, ITypeHandler4 arrayElementHandler, IReadBuffer
				 buffer, QCandidates candidates)
			{
				this._enclosing = _enclosing;
				this.arrayElementHandler = arrayElementHandler;
				this.buffer = buffer;
				this.candidates = candidates;
			}

			public object Run()
			{
				QueryingReadContext context = null;
				if (Handlers4.HandleAsObject(arrayElementHandler))
				{
					// TODO: Code is similar to FieldMetadata.collectIDs. Try to refactor to one place.
					int collectionID = buffer.ReadInt();
					ByteArrayBuffer arrayElementBuffer = this._enclosing.Container().ReadBufferById(this
						._enclosing.Transaction(), collectionID);
					ObjectHeader objectHeader = ObjectHeader.ScrollBufferToContent(this._enclosing.Container
						(), arrayElementBuffer);
					context = new QueryingReadContext(this._enclosing.Transaction(), candidates, this
						._enclosing._handlerVersion, arrayElementBuffer, collectionID);
					objectHeader.ClassMetadata().CollectIDs(context);
				}
				else
				{
					context = new QueryingReadContext(this._enclosing.Transaction(), candidates, this
						._enclosing._handlerVersion, buffer, 0);
					((ICascadingTypeHandler)arrayElementHandler).CollectIDs(context);
				}
				Tree.Traverse(context.Ids(), new _IVisitor4_223(candidates));
				IEnumerator i = context.ObjectsWithoutId();
				while (i.MoveNext())
				{
					object obj = i.Current;
					candidates.Add(new Db4objects.Db4o.Internal.Query.Processor.QCandidate(candidates
						, obj, 0));
				}
				return null;
			}

			private sealed class _IVisitor4_223 : IVisitor4
			{
				public _IVisitor4_223(QCandidates candidates)
				{
					this.candidates = candidates;
				}

				public void Visit(object obj)
				{
					TreeInt idNode = (TreeInt)obj;
					candidates.Add(new Db4objects.Db4o.Internal.Query.Processor.QCandidate(candidates
						, null, idNode._key));
				}

				private readonly QCandidates candidates;
			}

			private readonly QCandidate _enclosing;

			private readonly ITypeHandler4 arrayElementHandler;

			private readonly IReadBuffer buffer;

			private readonly QCandidates candidates;
		}

		internal virtual IReflectClass ClassReflector()
		{
			ClassMetadata();
			if (_classMetadata == null)
			{
				return null;
			}
			return _classMetadata.ClassReflector();
		}

		public override bool FieldIsAvailable()
		{
			return ClassReflector() != null;
		}

		internal virtual IReflectClass MemberClass()
		{
			return Transaction().Reflector().ForObject(_member);
		}

		private void Read()
		{
			if (_include)
			{
				if (_bytes == null)
				{
					if (_key > 0)
					{
						if (DTrace.enabled)
						{
							DTrace.CandidateRead.Log(_key);
						}
						SetBytes(Container().ReadBufferById(Transaction(), _key));
						if (_bytes == null)
						{
							Include(false);
						}
					}
					else
					{
						Include(false);
					}
				}
			}
		}

		private int CurrentOffSet()
		{
			return _bytes._offset;
		}

		private IInternalCandidate ReadSubCandidate(QCandidates candidateCollection)
		{
			Read();
			if (_bytes == null || _fieldMetadata == null)
			{
				return null;
			}
			int offset = CurrentOffSet();
			QueryingReadContext context = NewQueryingReadContext();
			ITypeHandler4 handler = HandlerRegistry.CorrectHandlerVersion(context, _fieldMetadata
				.GetHandler());
			IInternalCandidate subCandidate = candidateCollection.ReadSubCandidate(context, handler
				);
			Seek(offset);
			if (subCandidate != null)
			{
				subCandidate.Root(GetRoot());
				return subCandidate;
			}
			return null;
		}

		private void Seek(int offset)
		{
			_bytes._offset = offset;
		}

		private QueryingReadContext NewQueryingReadContext()
		{
			return new QueryingReadContext(Transaction(), _handlerVersion, _bytes, _key);
		}

		private void ReadThis(bool a_activate)
		{
			Read();
			ObjectContainerBase container = Transaction().Container();
			_member = container.TryGetByID(Transaction(), _key);
			if (_member != null && (a_activate || _member is ICompare))
			{
				container.Activate(Transaction(), _member);
				CheckInstanceOfCompare();
			}
		}

		public override Db4objects.Db4o.Internal.ClassMetadata ClassMetadata()
		{
			if (_classMetadata != null)
			{
				return _classMetadata;
			}
			Read();
			if (_bytes == null)
			{
				return null;
			}
			Seek(0);
			ObjectContainerBase stream = Container();
			ObjectHeader objectHeader = new ObjectHeader(stream, _bytes);
			_classMetadata = objectHeader.ClassMetadata();
			if (_classMetadata != null)
			{
				if (stream._handlers.IclassCompare.IsAssignableFrom(_classMetadata.ClassReflector
					()))
				{
					ReadThis(false);
				}
			}
			return _classMetadata;
		}

		public override string ToString()
		{
			string str = "QCandidate id: " + _key;
			if (_classMetadata != null)
			{
				str += "\n   YapClass " + _classMetadata.GetName();
			}
			if (_fieldMetadata != null)
			{
				str += "\n   YapField " + _fieldMetadata.GetName();
			}
			if (_member != null)
			{
				str += "\n   Member " + _member.ToString();
			}
			if (_root != null)
			{
				str += "\n  rooted by:\n";
				str += _root.ToString();
			}
			else
			{
				str += "\n  ROOT";
			}
			return str;
		}

		public virtual void UseField(QField a_field)
		{
			Read();
			if (_bytes == null)
			{
				_fieldMetadata = null;
				return;
			}
			ClassMetadata();
			_member = null;
			if (a_field == null)
			{
				_fieldMetadata = null;
				return;
			}
			if (_classMetadata == null)
			{
				_fieldMetadata = null;
				return;
			}
			_fieldMetadata = FieldMetadataFrom(a_field, _classMetadata);
			if (_fieldMetadata == null)
			{
				FieldNotFound();
				return;
			}
			HandlerVersion handlerVersion = _classMetadata.SeekToField(Transaction(), _bytes, 
				_fieldMetadata);
			if (handlerVersion == HandlerVersion.Invalid)
			{
				FieldNotFound();
				return;
			}
			_handlerVersion = handlerVersion._number;
		}

		private FieldMetadata FieldMetadataFrom(QField qField, Db4objects.Db4o.Internal.ClassMetadata
			 type)
		{
			FieldMetadata existingField = qField.GetFieldMetadata();
			if (existingField != null)
			{
				return existingField;
			}
			FieldMetadata field = type.FieldMetadataForName(qField.Name());
			if (field != null)
			{
				field.Alive();
			}
			return field;
		}

		private void FieldNotFound()
		{
			if (_classMetadata.HoldsAnyClass())
			{
				// retry finding the field on reading the value 
				_fieldMetadata = null;
			}
			else
			{
				// we can't get a value for the field, comparisons should definitely run against null
				_fieldMetadata = new NullFieldMetadata();
			}
			_handlerVersion = HandlerRegistry.HandlerVersion;
		}

		internal virtual object Value()
		{
			return Value(false);
		}

		// TODO: This is only used for Evaluations. Handling may need
		// to be different for collections also.
		internal virtual object Value(bool a_activate)
		{
			if (_member == null)
			{
				if (_fieldMetadata == null)
				{
					ReadThis(a_activate);
				}
				else
				{
					int offset = CurrentOffSet();
					_member = _fieldMetadata.Read(NewQueryingReadContext());
					Seek(offset);
					CheckInstanceOfCompare();
				}
			}
			return _member;
		}

		internal virtual void SetBytes(ByteArrayBuffer bytes)
		{
			_bytes = bytes;
		}

		private Db4objects.Db4o.Internal.Marshall.MarshallerFamily MarshallerFamily()
		{
			return Db4objects.Db4o.Internal.Marshall.MarshallerFamily.Version(_handlerVersion
				);
		}

		public virtual void ClassMetadata(Db4objects.Db4o.Internal.ClassMetadata classMetadata
			)
		{
			_classMetadata = classMetadata;
		}

		public override bool Evaluate(QConObject a_constraint, QE a_evaluator)
		{
			if (a_evaluator.Identity())
			{
				return a_evaluator.Evaluate(a_constraint, this, null);
			}
			if (_member == null)
			{
				_member = Value();
			}
			return a_evaluator.Evaluate(a_constraint, this, a_constraint.Translate(_member));
		}

		public override object GetObject()
		{
			object obj = Value(true);
			if (obj is ByteArrayBuffer)
			{
				ByteArrayBuffer reader = (ByteArrayBuffer)obj;
				int offset = reader._offset;
				obj = StringHandler.ReadString(Transaction().Context(), reader);
				reader._offset = offset;
			}
			return obj;
		}

		public override IPreparedComparison PrepareComparison(ObjectContainerBase container
			, object constraint)
		{
			IContext context = container.Transaction.Context();
			if (_fieldMetadata != null)
			{
				return _fieldMetadata.PrepareComparison(context, constraint);
			}
			if (_classMetadata != null)
			{
				return _classMetadata.PrepareComparison(context, constraint);
			}
			IReflector reflector = container.Reflector();
			Db4objects.Db4o.Internal.ClassMetadata classMetadata = null;
			if (_bytes != null)
			{
				classMetadata = container.ProduceClassMetadata(reflector.ForObject(constraint));
			}
			else
			{
				if (_member != null)
				{
					classMetadata = container.ClassMetadataForReflectClass(reflector.ForObject(_member
						));
				}
			}
			if (classMetadata != null)
			{
				if (_member != null && _member.GetType().IsArray)
				{
					ITypeHandler4 arrayElementTypehandler = classMetadata.TypeHandler();
					if (reflector.Array().IsNDimensional(MemberClass()))
					{
						MultidimensionalArrayHandler mah = new MultidimensionalArrayHandler(arrayElementTypehandler
							, false);
						return mah.PrepareComparison(context, _member);
					}
					ArrayHandler ya = new ArrayHandler(arrayElementTypehandler, false);
					return ya.PrepareComparison(context, _member);
				}
				return classMetadata.PrepareComparison(context, constraint);
			}
			return null;
		}

		internal sealed class CreateDescendChildTraversingVisitor : IVisitor4
		{
			private readonly ByRef _pending;

			private readonly BooleanByRef _innerRes;

			private readonly bool _isNot;

			internal CreateDescendChildTraversingVisitor(ByRef pending, BooleanByRef innerRes
				, bool isNot)
			{
				_pending = pending;
				_innerRes = innerRes;
				_isNot = isNot;
			}

			public void Visit(object obj)
			{
				IInternalCandidate cand = (IInternalCandidate)obj;
				if (cand.Include())
				{
					_innerRes.value = !_isNot;
				}
				// Collect all pending subresults.
				if (cand.PendingJoins() == null)
				{
					return;
				}
				cand.PendingJoins().Traverse(new _IVisitor4_529(this));
			}

			private sealed class _IVisitor4_529 : IVisitor4
			{
				public _IVisitor4_529(CreateDescendChildTraversingVisitor _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public void Visit(object a_object)
				{
					QPending newPending = ((QPending)a_object).InternalClonePayload();
					// We need to change the constraint here, so our pending collector
					// uses the right comparator.
					newPending.ChangeConstraint();
					QPending oldPending = (QPending)Tree.Find(((Tree)this._enclosing._pending.value), 
						newPending);
					if (oldPending != null)
					{
						// We only keep one pending result for all array elements and memorize,
						// whether we had a true or a false result or both.
						if (oldPending._result != newPending._result)
						{
							oldPending._result = QPending.Both;
						}
					}
					else
					{
						this._enclosing._pending.value = Tree.Add(((Tree)this._enclosing._pending.value), 
							newPending);
					}
				}

				private readonly CreateDescendChildTraversingVisitor _enclosing;
			}
		}
	}
}
