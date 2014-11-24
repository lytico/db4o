/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Classindex;

namespace Db4objects.Db4o.Internal.Classindex
{
	/// <exclude></exclude>
	public abstract class AbstractClassIndexStrategy : IClassIndexStrategy
	{
		protected readonly ClassMetadata _classMetadata;

		public AbstractClassIndexStrategy(ClassMetadata classMetadata)
		{
			_classMetadata = classMetadata;
		}

		protected virtual int ClassMetadataID()
		{
			return _classMetadata.GetID();
		}

		public virtual int OwnLength()
		{
			return Const4.IdLength;
		}

		protected abstract void InternalAdd(Transaction trans, int id);

		public void Add(Transaction trans, int id)
		{
			if (DTrace.enabled)
			{
				DTrace.AddToClassIndex.Log(id);
			}
			CheckId(id);
			InternalAdd(trans, id);
		}

		protected abstract void InternalRemove(Transaction ta, int id);

		public void Remove(Transaction ta, int id)
		{
			if (DTrace.enabled)
			{
				DTrace.RemoveFromClassIndex.Log(id);
			}
			CheckId(id);
			InternalRemove(ta, id);
		}

		private void CheckId(int id)
		{
		}

		public abstract IEnumerator AllSlotIDs(Transaction arg1);

		public abstract void DefragIndex(DefragmentContextImpl arg1);

		public abstract void DefragReference(ClassMetadata arg1, DefragmentContextImpl arg2
			, int arg3);

		public abstract void DontDelete(Transaction arg1, int arg2);

		public abstract int EntryCount(Transaction arg1);

		public abstract int Id();

		public abstract IIntVisitable IdVisitable(Transaction arg1);

		public abstract void Initialize(ObjectContainerBase arg1);

		public abstract void Purge();

		public abstract void Read(ObjectContainerBase arg1, int arg2);

		public abstract void TraverseIds(Transaction arg1, IVisitor4 arg2);

		public abstract int Write(Transaction arg1);
	}
}
