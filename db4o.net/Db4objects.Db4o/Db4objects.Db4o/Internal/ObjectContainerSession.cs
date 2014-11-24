/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Callbacks;
using Db4objects.Db4o.Internal.Events;
using Db4objects.Db4o.Internal.Qlin;
using Db4objects.Db4o.Internal.Query;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;
using Db4objects.Db4o.Types;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public partial class ObjectContainerSession : IInternalObjectContainer, ITransientClass
		, IObjectContainerSpec
	{
		protected readonly ObjectContainerBase _server;

		protected readonly Db4objects.Db4o.Internal.Transaction _transaction;

		private bool _closed = false;

		public ObjectContainerSession(ObjectContainerBase server, Db4objects.Db4o.Internal.Transaction
			 trans)
		{
			_server = server;
			_transaction = trans;
		}

		public ObjectContainerSession(ObjectContainerBase server) : this(server, server.NewUserTransaction
			())
		{
			_transaction.SetOutSideRepresentation(this);
		}

		/// <param name="path"></param>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="System.NotSupportedException"></exception>
		public virtual void Backup(string path)
		{
			throw new NotSupportedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="System.NotSupportedException"></exception>
		public virtual void Backup(IStorage storage, string path)
		{
			throw new NotSupportedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.InvalidIDException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual void Bind(object obj, long id)
		{
			_server.Bind(_transaction, obj, id);
		}

		public virtual Config4Impl ConfigImpl
		{
			get
			{
				// internal interface method doesn't need to lock
				return _server.ConfigImpl;
			}
		}

		public virtual IConfiguration Configure()
		{
			// FIXME: Consider throwing NotSupportedException here.
			// throw new NotSupportedException();
			lock (Lock())
			{
				CheckClosed();
				return _server.Configure();
			}
		}

		public virtual object Descend(object obj, string[] path)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.Descend(_transaction, obj, path);
			}
		}

		private void CheckClosed()
		{
			if (IsClosed())
			{
				throw new DatabaseClosedException();
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.InvalidIDException"></exception>
		public virtual object GetByID(long id)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.GetByID(_transaction, id);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual object GetByUUID(Db4oUUID uuid)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.GetByUUID(_transaction, uuid);
			}
		}

		public virtual long GetID(object obj)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.GetID(_transaction, obj);
			}
		}

		public virtual IObjectInfo GetObjectInfo(object obj)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.GetObjectInfo(_transaction, obj);
			}
		}

		// TODO: Db4oDatabase is shared between embedded clients.
		// This should work, since there is an automatic bind
		// replacement. Replication test cases will tell.
		public virtual Db4oDatabase Identity()
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.Identity();
			}
		}

		public virtual bool IsActive(object obj)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.IsActive(_transaction, obj);
			}
		}

		public virtual bool IsCached(long id)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.IsCached(_transaction, id);
			}
		}

		public virtual bool IsClosed()
		{
			lock (Lock())
			{
				return _closed == true;
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual bool IsStored(object obj)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.IsStored(_transaction, obj);
			}
		}

		public virtual IReflectClass[] KnownClasses()
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.KnownClasses();
			}
		}

		public virtual object Lock()
		{
			return _server.Lock();
		}

		public virtual object PeekPersisted(object @object, int depth, bool committed)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.PeekPersisted(_transaction, @object, ActivationDepthProvider().ActivationDepth
					(depth, ActivationMode.Peek), committed);
			}
		}

		public virtual void Purge()
		{
			lock (Lock())
			{
				CheckClosed();
				_server.Purge();
			}
		}

		public virtual void Purge(object obj)
		{
			lock (Lock())
			{
				CheckClosed();
				_server.Purge(_transaction, obj);
			}
		}

		public virtual GenericReflector Reflector()
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.Reflector();
			}
		}

		public virtual void Refresh(object obj, int depth)
		{
			lock (Lock())
			{
				CheckClosed();
				_server.Refresh(_transaction, obj, depth);
			}
		}

		public virtual void ReleaseSemaphore(string name)
		{
			CheckClosed();
			_server.ReleaseSemaphore(_transaction, name);
		}

		public virtual void Store(object obj, int depth)
		{
			lock (Lock())
			{
				CheckClosed();
				_server.Store(_transaction, obj, (depth == Const4.Unspecified ? (IUpdateDepth)UpdateDepthProvider
					().Unspecified(NullModifiedObjectQuery.Instance) : (IUpdateDepth)UpdateDepthProvider
					().ForDepth(depth)));
			}
		}

		public virtual bool SetSemaphore(string name, int waitForAvailability)
		{
			CheckClosed();
			return _server.SetSemaphore(_transaction, name, waitForAvailability);
		}

		public virtual IStoredClass StoredClass(object clazz)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.StoredClass(_transaction, clazz);
			}
		}

		public virtual IStoredClass[] StoredClasses()
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.StoredClasses(_transaction);
			}
		}

		public virtual ISystemInfo SystemInfo()
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.SystemInfo();
			}
		}

		public virtual long Version()
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.Version();
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual void Activate(object obj)
		{
			lock (Lock())
			{
				CheckClosed();
				_server.Activate(_transaction, obj);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual void Activate(object obj, int depth)
		{
			lock (Lock())
			{
				CheckClosed();
				_server.Activate(_transaction, obj, ActivationDepthProvider().ActivationDepth(depth
					, ActivationMode.Activate));
			}
		}

		private IActivationDepthProvider ActivationDepthProvider()
		{
			return _server.ActivationDepthProvider();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual bool Close()
		{
			lock (Lock())
			{
				if (IsClosed())
				{
					return false;
				}
				if (!_server.IsClosed())
				{
					if (!_server.ConfigImpl.IsReadOnly())
					{
						Commit();
					}
				}
				_server.Callbacks().CloseOnStarted(this);
				_server.CloseTransaction(_transaction, false, false);
				_closed = true;
				return true;
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		/// <exception cref="Db4objects.Db4o.Constraints.UniqueFieldValueConstraintViolationException
		/// 	"></exception>
		public virtual void Commit()
		{
			lock (Lock())
			{
				CheckClosed();
				_server.Commit(_transaction);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual void Deactivate(object obj, int depth)
		{
			lock (Lock())
			{
				CheckClosed();
				_server.Deactivate(_transaction, obj, depth);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual void Deactivate(object obj)
		{
			Deactivate(obj, 1);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public virtual void Delete(object obj)
		{
			lock (Lock())
			{
				CheckClosed();
				_server.Delete(_transaction, obj);
			}
		}

		public virtual IExtObjectContainer Ext()
		{
			return (IExtObjectContainer)this;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual IObjectSet QueryByExample(object template)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.QueryByExample(_transaction, template);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual IQuery Query()
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.Query(_transaction);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual IObjectSet Query(Type clazz)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.Query(_transaction, clazz);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual IObjectSet Query(Predicate predicate)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.Query(_transaction, predicate);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual IObjectSet Query(Predicate predicate, IQueryComparator comparator)
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.Query(_transaction, predicate, comparator);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public virtual void Rollback()
		{
			lock (Lock())
			{
				CheckClosed();
				_server.Rollback(_transaction);
			}
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public virtual void Store(object obj)
		{
			lock (Lock())
			{
				CheckClosed();
				_server.Store(_transaction, obj);
			}
		}

		public virtual ObjectContainerBase Container
		{
			get
			{
				return _server;
			}
		}

		public virtual Db4objects.Db4o.Internal.Transaction Transaction
		{
			get
			{
				return _transaction;
			}
		}

		public virtual void Callbacks(ICallbacks cb)
		{
			lock (Lock())
			{
				CheckClosed();
				_server.Callbacks(cb);
			}
		}

		public virtual ICallbacks Callbacks()
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.Callbacks();
			}
		}

		public NativeQueryHandler GetNativeQueryHandler()
		{
			lock (Lock())
			{
				CheckClosed();
				return _server.GetNativeQueryHandler();
			}
		}

		public virtual ClassMetadata ClassMetadataForReflectClass(IReflectClass reflectClass
			)
		{
			return _server.ClassMetadataForReflectClass(reflectClass);
		}

		public virtual ClassMetadata ClassMetadataForName(string name)
		{
			return _server.ClassMetadataForName(name);
		}

		public virtual ClassMetadata ClassMetadataForID(int id)
		{
			return _server.ClassMetadataForID(id);
		}

		public virtual HandlerRegistry Handlers
		{
			get
			{
				return _server.Handlers;
			}
		}

		public virtual object SyncExec(IClosure4 block)
		{
			return _server.SyncExec(block);
		}

		public virtual int InstanceCount(ClassMetadata clazz, Db4objects.Db4o.Internal.Transaction
			 trans)
		{
			return _server.InstanceCount(clazz, trans);
		}

		public virtual bool IsClient
		{
			get
			{
				return true;
			}
		}

		public virtual void StoreAll(Db4objects.Db4o.Internal.Transaction transaction, IEnumerator
			 objects)
		{
			_server.StoreAll(transaction, objects);
		}

		public virtual IUpdateDepthProvider UpdateDepthProvider()
		{
			return ConfigImpl.UpdateDepthProvider();
		}

		public virtual IObjectContainer OpenSession()
		{
			lock (Lock())
			{
				return new Db4objects.Db4o.Internal.ObjectContainerSession(_server);
			}
		}

		public virtual EventRegistryImpl NewEventRegistry()
		{
			return new EventRegistryImpl();
		}

		public virtual IQLin From(Type clazz)
		{
			return new QLinRoot(Query(), clazz);
		}

		public virtual bool InCallback()
		{
			return EventRegistryImpl.InCallback(this);
		}
	}
}
