/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class LazyObjectReference : IObjectInfo
	{
		private readonly Transaction _transaction;

		private readonly int _id;

		public LazyObjectReference(Transaction transaction, int id)
		{
			_transaction = transaction;
			_id = id;
		}

		public virtual long GetInternalID()
		{
			return _id;
		}

		public virtual object GetObject()
		{
			lock (ContainerLock())
			{
				return Reference().GetObject();
			}
		}

		public virtual Db4oUUID GetUUID()
		{
			lock (ContainerLock())
			{
				return Reference().GetUUID();
			}
		}

		public virtual long GetVersion()
		{
			return GetCommitTimestamp();
		}

		public virtual long GetCommitTimestamp()
		{
			lock (ContainerLock())
			{
				return Reference().GetCommitTimestamp();
			}
		}

		public virtual ObjectReference Reference()
		{
			HardObjectReference hardRef = _transaction.Container().GetHardObjectReferenceById
				(_transaction, _id);
			return hardRef._reference;
		}

		private object ContainerLock()
		{
			_transaction.Container().CheckClosed();
			return _transaction.Container().Lock();
		}
	}
}
