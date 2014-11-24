/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Callbacks;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Callbacks
{
	public class NullCallbacks : ICallbacks
	{
		public virtual void QueryOnFinished(Transaction transaction, IQuery query)
		{
		}

		public virtual void QueryOnStarted(Transaction transaction, IQuery query)
		{
		}

		public virtual bool ObjectCanNew(Transaction transaction, object obj)
		{
			return true;
		}

		public virtual bool ObjectCanActivate(Transaction transaction, object obj)
		{
			return true;
		}

		public virtual bool ObjectCanUpdate(Transaction transaction, IObjectInfo objectInfo
			)
		{
			return true;
		}

		public virtual bool ObjectCanDelete(Transaction transaction, IObjectInfo objectInfo
			)
		{
			return true;
		}

		public virtual bool ObjectCanDeactivate(Transaction transaction, IObjectInfo objectInfo
			)
		{
			return true;
		}

		public virtual void ObjectOnNew(Transaction transaction, IObjectInfo obj)
		{
		}

		public virtual void ObjectOnActivate(Transaction transaction, IObjectInfo obj)
		{
		}

		public virtual void ObjectOnUpdate(Transaction transaction, IObjectInfo obj)
		{
		}

		public virtual void ObjectOnDelete(Transaction transaction, IObjectInfo obj)
		{
		}

		public virtual void ObjectOnDeactivate(Transaction transaction, IObjectInfo obj)
		{
		}

		public virtual void ObjectOnInstantiate(Transaction transaction, IObjectInfo obj)
		{
		}

		public virtual void CommitOnStarted(Transaction transaction, CallbackObjectInfoCollections
			 objectInfoCollections)
		{
		}

		public virtual void CommitOnCompleted(Transaction transaction, CallbackObjectInfoCollections
			 objectInfoCollections, bool isOwnCommit)
		{
		}

		public virtual bool CaresAboutCommitting()
		{
			return false;
		}

		public virtual bool CaresAboutCommitted()
		{
			return false;
		}

		public virtual void ClassOnRegistered(ClassMetadata clazz)
		{
		}

		public virtual bool CaresAboutDeleting()
		{
			return false;
		}

		public virtual bool CaresAboutDeleted()
		{
			return false;
		}

		public virtual void CloseOnStarted(IObjectContainer container)
		{
		}

		public virtual void OpenOnFinished(IObjectContainer container)
		{
		}
	}
}
