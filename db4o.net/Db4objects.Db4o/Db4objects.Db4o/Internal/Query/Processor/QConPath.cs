/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <summary>
	/// Placeholder for a constraint, only necessary to attach children
	/// to the query graph.
	/// </summary>
	/// <remarks>
	/// Placeholder for a constraint, only necessary to attach children
	/// to the query graph.
	/// Added upon a call to Query#descend(), if there is no
	/// other place to hook up a new constraint.
	/// </remarks>
	/// <exclude></exclude>
	public class QConPath : QConClass
	{
		public QConPath()
		{
		}

		internal QConPath(Transaction a_trans, QCon a_parent, QField a_field) : base(a_trans
			, a_parent, a_field, null)
		{
			if (a_field != null)
			{
				_classMetadata = a_field.GetFieldType();
			}
		}

		public override bool CanLoadByIndex()
		{
			return false;
		}

		internal override bool Evaluate(IInternalCandidate candidate)
		{
			if (!candidate.FieldIsAvailable())
			{
				VisitOnNull(candidate.GetRoot());
			}
			return true;
		}

		internal override void EvaluateSelf()
		{
		}

		// do nothing
		internal override bool IsNullConstraint()
		{
			return !HasChildren();
		}

		internal override QConClass ShareParentForClass(IReflectClass a_class, BooleanByRef
			 removeExisting)
		{
			if (i_parent == null)
			{
				return null;
			}
			QConClass newConstraint = new QConClass(i_trans, i_parent, GetField(), a_class);
			Morph(removeExisting, newConstraint, a_class);
			return newConstraint;
		}

		internal override QCon ShareParent(object a_object, BooleanByRef removeExisting)
		{
			if (i_parent == null)
			{
				return null;
			}
			object obj = GetField().Coerce(a_object);
			if (obj == No4.Instance)
			{
				QCon falseConstraint = new QConUnconditional(i_trans, false);
				Morph(removeExisting, falseConstraint, ReflectClassForObject(obj));
				return falseConstraint;
			}
			QConObject newConstraint = new QConObject(i_trans, i_parent, GetField(), obj);
			Morph(removeExisting, newConstraint, ReflectClassForObject(obj));
			return newConstraint;
		}

		private IReflectClass ReflectClassForObject(object obj)
		{
			return i_trans.Reflector().ForObject(obj);
		}

		// Our QConPath objects are just placeholders to fields,
		// so the parents are reachable.
		// If we find a "real" constraint, we throw the QPath
		// out and replace it with the other constraint. 
		private void Morph(BooleanByRef removeExisting, QCon newConstraint, IReflectClass
			 claxx)
		{
			bool mayMorph = true;
			if (claxx != null)
			{
				ClassMetadata yc = i_trans.Container().ProduceClassMetadata(claxx);
				if (yc != null)
				{
					IEnumerator i = IterateChildren();
					while (i.MoveNext())
					{
						QField qf = ((QCon)i.Current).GetField();
						if (!yc.HasField(i_trans.Container(), qf.Name()))
						{
							mayMorph = false;
							break;
						}
					}
				}
			}
			if (mayMorph)
			{
				IEnumerator j = IterateChildren();
				while (j.MoveNext())
				{
					newConstraint.AddConstraint((QCon)j.Current);
				}
				if (HasJoins())
				{
					IEnumerator k = IterateJoins();
					while (k.MoveNext())
					{
						QConJoin qcj = (QConJoin)k.Current;
						qcj.ExchangeConstraint(this, newConstraint);
						newConstraint.AddJoin(qcj);
					}
				}
				i_parent.ExchangeConstraint(this, newConstraint);
				removeExisting.value = true;
			}
			else
			{
				i_parent.AddConstraint(newConstraint);
			}
		}

		internal sealed override bool VisitSelfOnNull()
		{
			return false;
		}

		public override string ToString()
		{
			return "QConPath " + base.ToString();
		}

		public override void SetProcessedByIndex(QCandidates candidates)
		{
			if (ChildrenCount() <= 1)
			{
				InternalSetProcessedByIndex(null);
			}
		}

		protected override bool CanResolveByFieldIndex()
		{
			return true;
		}
	}
}
