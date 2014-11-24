/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;

namespace Db4objects.Db4o.Internal.References
{
	/// <exclude></exclude>
	public class TransactionalReferenceSystem : TransactionalReferenceSystemBase, IReferenceSystem
	{
		public override void Commit()
		{
			TraverseNewReferences(new _IVisitor4_16(this));
			CreateNewReferences();
		}

		private sealed class _IVisitor4_16 : IVisitor4
		{
			public _IVisitor4_16(TransactionalReferenceSystem _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object obj)
			{
				ObjectReference oref = (ObjectReference)obj;
				if (oref.GetObject() != null)
				{
					this._enclosing._committedReferences.AddExistingReference(oref);
				}
			}

			private readonly TransactionalReferenceSystem _enclosing;
		}

		public override void AddExistingReference(ObjectReference @ref)
		{
			_committedReferences.AddExistingReference(@ref);
		}

		public override void AddNewReference(ObjectReference @ref)
		{
			_newReferences.AddNewReference(@ref);
		}

		public override void RemoveReference(ObjectReference @ref)
		{
			_newReferences.RemoveReference(@ref);
			_committedReferences.RemoveReference(@ref);
		}

		public override void Rollback()
		{
			CreateNewReferences();
		}

		public virtual void Discarded()
		{
		}
		// do nothing;
	}
}
