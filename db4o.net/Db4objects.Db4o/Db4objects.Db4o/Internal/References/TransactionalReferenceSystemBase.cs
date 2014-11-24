/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;

namespace Db4objects.Db4o.Internal.References
{
	/// <exclude></exclude>
	public abstract class TransactionalReferenceSystemBase
	{
		protected readonly IReferenceSystem _committedReferences;

		protected IReferenceSystem _newReferences;

		public TransactionalReferenceSystemBase()
		{
			CreateNewReferences();
			_committedReferences = NewReferenceSystem();
		}

		private IReferenceSystem NewReferenceSystem()
		{
			return new HashcodeReferenceSystem();
		}

		public abstract void AddExistingReference(ObjectReference @ref);

		public abstract void AddNewReference(ObjectReference @ref);

		public abstract void Commit();

		protected virtual void TraverseNewReferences(IVisitor4 visitor)
		{
			_newReferences.TraverseReferences(visitor);
		}

		protected virtual void CreateNewReferences()
		{
			_newReferences = NewReferenceSystem();
		}

		public virtual ObjectReference ReferenceForId(int id)
		{
			ObjectReference @ref = _newReferences.ReferenceForId(id);
			if (@ref != null)
			{
				return @ref;
			}
			return _committedReferences.ReferenceForId(id);
		}

		public virtual ObjectReference ReferenceForObject(object obj)
		{
			ObjectReference @ref = _newReferences.ReferenceForObject(obj);
			if (@ref != null)
			{
				return @ref;
			}
			return _committedReferences.ReferenceForObject(obj);
		}

		public abstract void RemoveReference(ObjectReference @ref);

		public abstract void Rollback();

		public virtual void TraverseReferences(IVisitor4 visitor)
		{
			TraverseNewReferences(visitor);
			_committedReferences.TraverseReferences(visitor);
		}
	}
}
