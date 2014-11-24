/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <summary>Object constraint on queries</summary>
	/// <exclude></exclude>
	public class QConObject : QCon, IFieldFilterable
	{
		private object i_object;

		private int i_objectID;

		[System.NonSerialized]
		internal ClassMetadata _classMetadata;

		private int i_classMetadataID;

		private QField i_field;

		[System.NonSerialized]
		internal IPreparedComparison _preparedComparison;

		private IObjectAttribute i_attributeProvider;

		[System.NonSerialized]
		private bool _checkClassMetadataOnly = false;

		public QConObject()
		{
		}

		public QConObject(Transaction a_trans, QCon a_parent, QField a_field, object a_object
			) : base(a_trans)
		{
			// the constraining object
			// cache for the db4o object ID
			// the YapClass
			// needed for marshalling the request
			// C/S only
			i_parent = a_parent;
			if (a_object is ICompare)
			{
				a_object = ((ICompare)a_object).Compare();
			}
			i_object = a_object;
			i_field = a_field;
		}

		private void AssociateYapClass(Transaction a_trans, object a_object)
		{
			if (a_object == null)
			{
			}
			else
			{
				//It seems that we need not result the following field
				//i_object = null;
				//i_comparator = Null.INSTANCE;
				//i_classMetadata = null;
				// FIXME: Setting the YapClass to null will prevent index use
				// If the field is typed we can guess the right one with the
				// following line. However this does break some SODA test cases.
				// Revisit!
				//            if(i_field != null){
				//                i_classMetadata = i_field.getYapClass();
				//            }
				_classMetadata = a_trans.Container().ProduceClassMetadata(a_trans.Reflector().ForObject
					(a_object));
				if (_classMetadata != null)
				{
					i_object = _classMetadata.GetComparableObject(a_object);
					if (a_object != i_object)
					{
						i_attributeProvider = _classMetadata.Config().QueryAttributeProvider();
						_classMetadata = a_trans.Container().ProduceClassMetadata(a_trans.Reflector().ForObject
							(i_object));
					}
					if (_classMetadata != null)
					{
						_classMetadata.CollectConstraints(a_trans, this, i_object, new _IVisitor4_84(this
							));
					}
					else
					{
						AssociateYapClass(a_trans, null);
					}
				}
				else
				{
					AssociateYapClass(a_trans, null);
				}
			}
		}

		private sealed class _IVisitor4_84 : IVisitor4
		{
			public _IVisitor4_84(QConObject _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object obj)
			{
				this._enclosing.AddConstraint((QCon)obj);
			}

			private readonly QConObject _enclosing;
		}

		public override bool CanBeIndexLeaf()
		{
			return i_object == null || ((_classMetadata != null && _classMetadata.IsValueType
				()) || Evaluator().Identity());
		}

		public override bool CanLoadByIndex()
		{
			if (i_field == null)
			{
				return false;
			}
			if (i_field._fieldMetadata == null)
			{
				return false;
			}
			if (!i_field._fieldMetadata.HasIndex())
			{
				return false;
			}
			if (!i_evaluator.SupportsIndex())
			{
				return false;
			}
			return i_field._fieldMetadata.CanLoadByIndex();
		}

		internal override bool Evaluate(IInternalCandidate candidate)
		{
			try
			{
				return candidate.Evaluate(this, i_evaluator);
			}
			catch (Exception e)
			{
				return false;
			}
		}

		internal override void EvaluateEvaluationsExec(QCandidates a_candidates, bool rereadObject
			)
		{
			if (i_field.IsQueryLeaf())
			{
				bool hasEvaluation = false;
				IEnumerator i = IterateChildren();
				while (i.MoveNext())
				{
					if (i.Current is QConEvaluation)
					{
						hasEvaluation = true;
						break;
					}
				}
				if (hasEvaluation)
				{
					IEnumerator j = IterateChildren();
					while (j.MoveNext())
					{
						((QCon)j.Current).EvaluateEvaluationsExec(a_candidates, false);
					}
				}
			}
		}

		internal override void EvaluateSelf()
		{
			if (DTrace.enabled)
			{
				DTrace.EvaluateSelf.Log(Id());
			}
			if (_classMetadata != null)
			{
				if (!(_classMetadata is PrimitiveTypeMetadata))
				{
					if (!i_evaluator.Identity() && (_classMetadata.TypeHandler() is StandardReferenceTypeHandler
						))
					{
						_checkClassMetadataOnly = true;
					}
					object transactionalObject = _classMetadata.WrapWithTransactionContext(Transaction
						(), i_object);
					_preparedComparison = _classMetadata.PrepareComparison(Context(), transactionalObject
						);
				}
			}
			base.EvaluateSelf();
			_checkClassMetadataOnly = false;
		}

		private IContext Context()
		{
			return Transaction().Context();
		}

		internal override void Collect(QCandidates a_candidates)
		{
			if (i_field.IsClass())
			{
				a_candidates.Filter(i_field, i_candidates);
			}
		}

		internal override void EvaluateSimpleExec(QCandidates a_candidates)
		{
			// TODO: The following can be skipped if we used the index on
			//       this field to load the objects, if hasOrdering() is false
			if (i_field.IsQueryLeaf() || IsNullConstraint())
			{
				PrepareComparison(i_field);
				a_candidates.Filter(i_field, this);
			}
		}

		internal virtual IPreparedComparison PrepareComparison(IInternalCandidate candidate
			)
		{
			if (_preparedComparison != null)
			{
				return _preparedComparison;
			}
			return candidate.PrepareComparison(Container(), i_object);
		}

		internal override ClassMetadata GetYapClass()
		{
			return _classMetadata;
		}

		public override QField GetField()
		{
			return i_field;
		}

		internal virtual int GetObjectID()
		{
			if (i_objectID == 0)
			{
				i_objectID = i_trans.Container().GetID(i_trans, i_object);
				if (i_objectID == 0)
				{
					i_objectID = -1;
				}
			}
			return i_objectID;
		}

		public override bool HasObjectInParentPath(object obj)
		{
			if (obj == i_object)
			{
				return true;
			}
			return base.HasObjectInParentPath(obj);
		}

		public override int IdentityID()
		{
			if (i_evaluator.Identity())
			{
				int id = GetObjectID();
				if (id != 0)
				{
					if (!(i_evaluator is QENot))
					{
						return id;
					}
				}
			}
			return 0;
		}

		internal override bool IsNullConstraint()
		{
			return i_object == null;
		}

		internal override void Log(string indent)
		{
		}

		internal override string LogObject()
		{
			return string.Empty;
		}

		internal override void Marshall()
		{
			base.Marshall();
			GetObjectID();
			if (_classMetadata != null)
			{
				i_classMetadataID = _classMetadata.GetID();
			}
		}

		public override bool OnSameFieldAs(QCon other)
		{
			if (!(other is Db4objects.Db4o.Internal.Query.Processor.QConObject))
			{
				return false;
			}
			return i_field == ((Db4objects.Db4o.Internal.Query.Processor.QConObject)other).i_field;
		}

		internal virtual void PrepareComparison(QField a_field)
		{
			if (IsNullConstraint() & !a_field.IsArray())
			{
				_preparedComparison = Null.Instance;
			}
			else
			{
				_preparedComparison = a_field.PrepareComparison(Context(), i_object);
			}
		}

		internal override QCon ShareParent(object a_object, BooleanByRef removeExisting)
		{
			if (i_parent == null)
			{
				return null;
			}
			object obj = i_field.Coerce(a_object);
			if (obj == No4.Instance)
			{
				return null;
			}
			return i_parent.AddSharedConstraint(i_field, obj);
		}

		internal override QConClass ShareParentForClass(IReflectClass a_class, BooleanByRef
			 removeExisting)
		{
			if (i_parent == null)
			{
				return null;
			}
			QConClass newConstraint = new QConClass(i_trans, i_parent, i_field, a_class);
			i_parent.AddConstraint(newConstraint);
			return newConstraint;
		}

		internal object Translate(object candidate)
		{
			if (i_attributeProvider != null)
			{
				i_candidates.i_trans.Container().Activate(i_candidates.i_trans, candidate);
				return i_attributeProvider.Attribute(candidate);
			}
			return candidate;
		}

		internal override void Unmarshall(Transaction trans)
		{
			if (i_trans != null)
			{
				return;
			}
			base.Unmarshall(trans);
			if (i_object == null)
			{
				_preparedComparison = Null.Instance;
			}
			if (i_classMetadataID != 0)
			{
				_classMetadata = trans.Container().ClassMetadataForID(i_classMetadataID);
			}
			if (i_field != null)
			{
				i_field.Unmarshall(trans);
			}
			if (i_objectID > 0)
			{
				object obj = trans.Container().TryGetByID(trans, i_objectID);
				if (obj != null)
				{
					i_object = obj;
				}
			}
		}

		public override void Visit(object obj)
		{
			IInternalCandidate qc = (IInternalCandidate)obj;
			bool res = true;
			bool processed = false;
			if (_checkClassMetadataOnly)
			{
				ClassMetadata yc = qc.ClassMetadata();
				if (yc != null)
				{
					res = i_evaluator.Not(_classMetadata.GetHigherHierarchy(yc) == _classMetadata);
					processed = true;
				}
			}
			if (!processed)
			{
				res = Evaluate(qc);
			}
			Visit1(qc.GetRoot(), this, res);
		}

		public virtual void Filter(QField field, IParentCandidate candidate)
		{
			candidate.UseField(field);
			bool res = true;
			bool processed = false;
			if (_checkClassMetadataOnly)
			{
				ClassMetadata classMetadata = candidate.ClassMetadata();
				if (classMetadata != null)
				{
					res = i_evaluator.Not(_classMetadata.GetHigherHierarchy(classMetadata) == _classMetadata
						);
					processed = true;
				}
			}
			if (!processed)
			{
				res = Evaluate(candidate);
			}
			Visit1(candidate.GetRoot(), this, res);
		}

		public override IConstraint Contains()
		{
			lock (StreamLock())
			{
				i_evaluator = i_evaluator.Add(new QEContains(true));
				return this;
			}
		}

		public override IConstraint Equal()
		{
			lock (StreamLock())
			{
				i_evaluator = i_evaluator.Add(new QEEqual());
				return this;
			}
		}

		public override object GetObject()
		{
			return i_object;
		}

		public override IConstraint Greater()
		{
			lock (StreamLock())
			{
				i_evaluator = i_evaluator.Add(new QEGreater());
				return this;
			}
		}

		public override IConstraint Identity()
		{
			lock (StreamLock())
			{
				if (i_object == null)
				{
					return this;
				}
				GetObjectID();
				i_evaluator = i_evaluator.Add(new QEIdentity());
				return this;
			}
		}

		public override IConstraint ByExample()
		{
			lock (StreamLock())
			{
				AssociateYapClass(i_trans, i_object);
				return this;
			}
		}

		internal virtual void SetEvaluationMode()
		{
			if ((i_object == null) || EvaluationModeAlreadySet())
			{
				return;
			}
			int id = GetObjectID();
			if (id < 0)
			{
				ByExample();
			}
			else
			{
				_classMetadata = i_trans.Container().ProduceClassMetadata(i_trans.Reflector().ForObject
					(i_object));
				Identity();
			}
		}

		internal virtual bool EvaluationModeAlreadySet()
		{
			return _classMetadata != null;
		}

		public override IConstraint Like()
		{
			lock (StreamLock())
			{
				i_evaluator = i_evaluator.Add(new QEContains(false));
				return this;
			}
		}

		public override IConstraint Smaller()
		{
			lock (StreamLock())
			{
				i_evaluator = i_evaluator.Add(new QESmaller());
				return this;
			}
		}

		public override IConstraint StartsWith(bool caseSensitive)
		{
			lock (StreamLock())
			{
				i_evaluator = i_evaluator.Add(new QEStartsWith(caseSensitive));
				return this;
			}
		}

		public override IConstraint EndsWith(bool caseSensitive)
		{
			lock (StreamLock())
			{
				i_evaluator = i_evaluator.Add(new QEEndsWith(caseSensitive));
				return this;
			}
		}

		public override string ToString()
		{
			string str = "QConObject ";
			if (i_object != null)
			{
				str += i_object.ToString();
			}
			return str;
		}

		protected override void InternalSetProcessedByIndex(QCandidates candidates)
		{
			base.InternalSetProcessedByIndex(candidates);
			if (i_field == null)
			{
				return;
			}
			FieldMetadata fieldMetadata = i_field.GetFieldMetadata();
			if (!fieldMetadata.IsVirtual())
			{
				candidates.WasLoadedFromClassFieldIndex(true);
			}
		}

		protected override bool CanResolveByFieldIndex()
		{
			return false;
		}
	}
}
