/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Callbacks;
using Db4objects.Db4o.Internal.Events;
using Db4objects.Db4o.Query;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Events
{
	/// <exclude></exclude>
	public class EventRegistryImpl : ICallbacks, IEventRegistry
	{
		protected System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs> _queryStarted;

		protected System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs> _queryFinished;

		protected System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs> 
			_creating;

		protected System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs> 
			_activating;

		protected System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs> 
			_updating;

		protected System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs> 
			_deleting;

		protected System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs> 
			_deactivating;

		protected System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs> _created;

		protected System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs> _activated;

		protected System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs> _updated;

		protected System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs> _deleted;

		protected System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs> _deactivated;

		protected System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs> _committing;

		protected System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs> _committed;

		protected System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs> _instantiated;

		protected System.EventHandler<Db4objects.Db4o.Events.ClassEventArgs> _classRegistered;

		protected System.EventHandler<Db4objects.Db4o.Events.ObjectContainerEventArgs> _closing;

		protected System.EventHandler<Db4objects.Db4o.Events.ObjectContainerEventArgs> _opened;

		private bool _inCallback = false;

		// Callbacks implementation
		public virtual void QueryOnFinished(Transaction transaction, IQuery query)
		{
			if (!(_queryFinished != null))
			{
				return;
			}
			WithExceptionHandling(new _IRunnable_52(this, transaction, query));
		}

		private sealed class _IRunnable_52 : IRunnable
		{
			public _IRunnable_52(EventRegistryImpl _enclosing, Transaction transaction, IQuery
				 query)
			{
				this._enclosing = _enclosing;
				this.transaction = transaction;
				this.query = query;
			}

			public void Run()
			{
				if (null != this._enclosing._queryFinished) this._enclosing._queryFinished(null, 
					new QueryEventArgs(transaction, query));
			}

			private readonly EventRegistryImpl _enclosing;

			private readonly Transaction transaction;

			private readonly IQuery query;
		}

		public virtual void QueryOnStarted(Transaction transaction, IQuery query)
		{
			if (!(_queryStarted != null))
			{
				return;
			}
			WithExceptionHandling(new _IRunnable_61(this, transaction, query));
		}

		private sealed class _IRunnable_61 : IRunnable
		{
			public _IRunnable_61(EventRegistryImpl _enclosing, Transaction transaction, IQuery
				 query)
			{
				this._enclosing = _enclosing;
				this.transaction = transaction;
				this.query = query;
			}

			public void Run()
			{
				if (null != this._enclosing._queryStarted) this._enclosing._queryStarted(null, new 
					QueryEventArgs(transaction, query));
			}

			private readonly EventRegistryImpl _enclosing;

			private readonly Transaction transaction;

			private readonly IQuery query;
		}

		public virtual bool ObjectCanNew(Transaction transaction, object obj)
		{
			return TriggerCancellableObjectEventArgsInCallback(transaction, _creating, null, 
				obj);
		}

		public virtual bool ObjectCanActivate(Transaction transaction, object obj)
		{
			return TriggerCancellableObjectEventArgsInCallback(transaction, _activating, null
				, obj);
		}

		public virtual bool ObjectCanUpdate(Transaction transaction, IObjectInfo objectInfo
			)
		{
			return TriggerCancellableObjectEventArgsInCallback(transaction, _updating, objectInfo
				, objectInfo.GetObject());
		}

		public virtual bool ObjectCanDelete(Transaction transaction, IObjectInfo objectInfo
			)
		{
			return TriggerCancellableObjectEventArgsInCallback(transaction, _deleting, objectInfo
				, objectInfo.GetObject());
		}

		public virtual bool ObjectCanDeactivate(Transaction transaction, IObjectInfo objectInfo
			)
		{
			return TriggerCancellableObjectEventArgsInCallback(transaction, _deactivating, objectInfo
				, objectInfo.GetObject());
		}

		public virtual void ObjectOnActivate(Transaction transaction, IObjectInfo obj)
		{
			TriggerObjectInfoEventInCallback(transaction, _activated, obj);
		}

		public virtual void ObjectOnNew(Transaction transaction, IObjectInfo obj)
		{
			TriggerObjectInfoEventInCallback(transaction, _created, obj);
		}

		public virtual void ObjectOnUpdate(Transaction transaction, IObjectInfo obj)
		{
			TriggerObjectInfoEventInCallback(transaction, _updated, obj);
		}

		public virtual void ObjectOnDelete(Transaction transaction, IObjectInfo obj)
		{
			TriggerObjectInfoEventInCallback(transaction, _deleted, obj);
		}

		public virtual void ClassOnRegistered(ClassMetadata clazz)
		{
			if (!(_classRegistered != null))
			{
				return;
			}
			WithExceptionHandling(new _IRunnable_106(this, clazz));
		}

		private sealed class _IRunnable_106 : IRunnable
		{
			public _IRunnable_106(EventRegistryImpl _enclosing, ClassMetadata clazz)
			{
				this._enclosing = _enclosing;
				this.clazz = clazz;
			}

			public void Run()
			{
				if (null != this._enclosing._classRegistered) this._enclosing._classRegistered(null, 
					new ClassEventArgs(clazz));
			}

			private readonly EventRegistryImpl _enclosing;

			private readonly ClassMetadata clazz;
		}

		public virtual void ObjectOnDeactivate(Transaction transaction, IObjectInfo obj)
		{
			TriggerObjectInfoEventInCallback(transaction, _deactivated, obj);
		}

		public virtual void ObjectOnInstantiate(Transaction transaction, IObjectInfo obj)
		{
			TriggerObjectInfoEventInCallback(transaction, _instantiated, obj);
		}

		public virtual void CommitOnStarted(Transaction transaction, CallbackObjectInfoCollections
			 objectInfoCollections)
		{
			if (!(_committing != null))
			{
				return;
			}
			WithExceptionHandlingInCallback(new _IRunnable_123(this, transaction, objectInfoCollections
				));
		}

		private sealed class _IRunnable_123 : IRunnable
		{
			public _IRunnable_123(EventRegistryImpl _enclosing, Transaction transaction, CallbackObjectInfoCollections
				 objectInfoCollections)
			{
				this._enclosing = _enclosing;
				this.transaction = transaction;
				this.objectInfoCollections = objectInfoCollections;
			}

			public void Run()
			{
				if (null != this._enclosing._committing) this._enclosing._committing(null, new CommitEventArgs
					(transaction, objectInfoCollections, false));
			}

			private readonly EventRegistryImpl _enclosing;

			private readonly Transaction transaction;

			private readonly CallbackObjectInfoCollections objectInfoCollections;
		}

		public virtual void CommitOnCompleted(Transaction transaction, CallbackObjectInfoCollections
			 objectInfoCollections, bool isOwnCommit)
		{
			if (!(_committed != null))
			{
				return;
			}
			WithExceptionHandlingInCallback(new _IRunnable_134(this, transaction, objectInfoCollections
				, isOwnCommit));
		}

		private sealed class _IRunnable_134 : IRunnable
		{
			public _IRunnable_134(EventRegistryImpl _enclosing, Transaction transaction, CallbackObjectInfoCollections
				 objectInfoCollections, bool isOwnCommit)
			{
				this._enclosing = _enclosing;
				this.transaction = transaction;
				this.objectInfoCollections = objectInfoCollections;
				this.isOwnCommit = isOwnCommit;
			}

			public void Run()
			{
				if (null != this._enclosing._committed) this._enclosing._committed(null, new CommitEventArgs
					(transaction, objectInfoCollections, isOwnCommit));
			}

			private readonly EventRegistryImpl _enclosing;

			private readonly Transaction transaction;

			private readonly CallbackObjectInfoCollections objectInfoCollections;

			private readonly bool isOwnCommit;
		}

		public virtual void CloseOnStarted(IObjectContainer container)
		{
			if (!(_closing != null))
			{
				return;
			}
			WithExceptionHandlingInCallback(new _IRunnable_145(this, container));
		}

		private sealed class _IRunnable_145 : IRunnable
		{
			public _IRunnable_145(EventRegistryImpl _enclosing, IObjectContainer container)
			{
				this._enclosing = _enclosing;
				this.container = container;
			}

			public void Run()
			{
				if (null != this._enclosing._closing) this._enclosing._closing(null, new ObjectContainerEventArgs
					(container));
			}

			private readonly EventRegistryImpl _enclosing;

			private readonly IObjectContainer container;
		}

		public virtual void OpenOnFinished(IObjectContainer container)
		{
			if (!(_opened != null))
			{
				return;
			}
			WithExceptionHandlingInCallback(new _IRunnable_156(this, container));
		}

		private sealed class _IRunnable_156 : IRunnable
		{
			public _IRunnable_156(EventRegistryImpl _enclosing, IObjectContainer container)
			{
				this._enclosing = _enclosing;
				this.container = container;
			}

			public void Run()
			{
				if (null != this._enclosing._opened) this._enclosing._opened(null, new ObjectContainerEventArgs
					(container));
			}

			private readonly EventRegistryImpl _enclosing;

			private readonly IObjectContainer container;
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs> QueryFinished
		{
			add
			{
				_queryFinished = (System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs>)System.Delegate.Combine
					(_queryFinished, value);
			}
			remove
			{
				_queryFinished = (System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs>)System.Delegate.Remove
					(_queryFinished, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs> QueryStarted
		{
			add
			{
				_queryStarted = (System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs>)System.Delegate.Combine
					(_queryStarted, value);
			}
			remove
			{
				_queryStarted = (System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs>)System.Delegate.Remove
					(_queryStarted, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
			 Creating
		{
			add
			{
				_creating = (System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					)System.Delegate.Combine(_creating, value);
			}
			remove
			{
				_creating = (System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					)System.Delegate.Remove(_creating, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
			 Activating
		{
			add
			{
				_activating = (System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					)System.Delegate.Combine(_activating, value);
			}
			remove
			{
				_activating = (System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					)System.Delegate.Remove(_activating, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
			 Updating
		{
			add
			{
				_updating = (System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					)System.Delegate.Combine(_updating, value);
			}
			remove
			{
				_updating = (System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					)System.Delegate.Remove(_updating, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
			 Deleting
		{
			add
			{
				_deleting = (System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					)System.Delegate.Combine(_deleting, value);
			}
			remove
			{
				_deleting = (System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					)System.Delegate.Remove(_deleting, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
			 Deactivating
		{
			add
			{
				_deactivating = (System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					)System.Delegate.Combine(_deactivating, value);
			}
			remove
			{
				_deactivating = (System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
					)System.Delegate.Remove(_deactivating, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
			 Created
		{
			add
			{
				_created = (System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>)System.Delegate.Combine
					(_created, value);
			}
			remove
			{
				_created = (System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>)System.Delegate.Remove
					(_created, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
			 Activated
		{
			add
			{
				_activated = (System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>)System.Delegate.Combine
					(_activated, value);
			}
			remove
			{
				_activated = (System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>)System.Delegate.Remove
					(_activated, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
			 Updated
		{
			add
			{
				_updated = (System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>)System.Delegate.Combine
					(_updated, value);
			}
			remove
			{
				_updated = (System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>)System.Delegate.Remove
					(_updated, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
			 Deleted
		{
			add
			{
				_deleted = (System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>)System.Delegate.Combine
					(_deleted, value);
			}
			remove
			{
				_deleted = (System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>)System.Delegate.Remove
					(_deleted, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
			 Deactivated
		{
			add
			{
				_deactivated = (System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>)System.Delegate.Combine
					(_deactivated, value);
			}
			remove
			{
				_deactivated = (System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>)System.Delegate.Remove
					(_deactivated, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs> 
			Committing
		{
			add
			{
				_committing = (System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>)System.Delegate.Combine
					(_committing, value);
			}
			remove
			{
				_committing = (System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>)System.Delegate.Remove
					(_committing, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs> 
			Committed
		{
			add
			{
				_committed = (System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>)System.Delegate.Combine
					(_committed, value);
				OnCommittedListenerAdded();
			}
			remove
			{
				_committed = (System.EventHandler<Db4objects.Db4o.Events.CommitEventArgs>)System.Delegate.Remove
					(_committed, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.ClassEventArgs> ClassRegistered
		{
			add
			{
				_classRegistered = (System.EventHandler<Db4objects.Db4o.Events.ClassEventArgs>)System.Delegate.Combine
					(_classRegistered, value);
			}
			remove
			{
				_classRegistered = (System.EventHandler<Db4objects.Db4o.Events.ClassEventArgs>)System.Delegate.Remove
					(_classRegistered, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
			 Instantiated
		{
			add
			{
				_instantiated = (System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>)
					System.Delegate.Combine(_instantiated, value);
			}
			remove
			{
				_instantiated = (System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>)
					System.Delegate.Remove(_instantiated, value);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.ObjectContainerEventArgs>
			 Closing
		{
			add
			{
				_closing = (System.EventHandler<Db4objects.Db4o.Events.ObjectContainerEventArgs>)
					System.Delegate.Combine(_closing, value);
			}
			remove
			{
				_closing = (System.EventHandler<Db4objects.Db4o.Events.ObjectContainerEventArgs>)
					System.Delegate.Remove(_closing, value);
			}
		}

		protected virtual void OnCommittedListenerAdded()
		{
		}

		// do nothing 
		public virtual bool CaresAboutCommitting()
		{
			return (_committing != null);
		}

		public virtual bool CaresAboutCommitted()
		{
			return (_committed != null);
		}

		public virtual bool CaresAboutDeleting()
		{
			return (_deleting != null);
		}

		public virtual bool CaresAboutDeleted()
		{
			return (_deleted != null);
		}

		internal virtual bool TriggerCancellableObjectEventArgsInCallback(Transaction transaction
			, System.EventHandler<CancellableObjectEventArgs> e, IObjectInfo objectInfo, object
			 o)
		{
			if (!(e != null))
			{
				return true;
			}
			CancellableObjectEventArgs args = new CancellableObjectEventArgs(transaction, objectInfo
				, o);
			WithExceptionHandlingInCallback(new _IRunnable_260(e, args));
			return !args.IsCancelled;
		}

		private sealed class _IRunnable_260 : IRunnable
		{
			public _IRunnable_260(System.EventHandler<CancellableObjectEventArgs> e, CancellableObjectEventArgs
				 args)
			{
				this.e = e;
				this.args = args;
			}

			public void Run()
			{
				if (null != e) e(null, args);
			}

			private readonly System.EventHandler<CancellableObjectEventArgs> e;

			private readonly CancellableObjectEventArgs args;
		}

		internal virtual void TriggerObjectInfoEventInCallback(Transaction transaction, System.EventHandler<
			ObjectInfoEventArgs> e, IObjectInfo o)
		{
			if (!(e != null))
			{
				return;
			}
			WithExceptionHandlingInCallback(new _IRunnable_272(e, transaction, o));
		}

		private sealed class _IRunnable_272 : IRunnable
		{
			public _IRunnable_272(System.EventHandler<ObjectInfoEventArgs> e, Transaction transaction
				, IObjectInfo o)
			{
				this.e = e;
				this.transaction = transaction;
				this.o = o;
			}

			public void Run()
			{
				if (null != e) e(null, new ObjectInfoEventArgs(transaction, o));
			}

			private readonly System.EventHandler<ObjectInfoEventArgs> e;

			private readonly Transaction transaction;

			private readonly IObjectInfo o;
		}

		private void WithExceptionHandlingInCallback(IRunnable runnable)
		{
			_inCallback = true;
			try
			{
				runnable.Run();
			}
			catch (Db4oException e)
			{
				throw;
			}
			catch (Exception x)
			{
				throw new EventException(x);
			}
			finally
			{
				_inCallback = false;
			}
		}

		private void WithExceptionHandling(IRunnable runnable)
		{
			try
			{
				runnable.Run();
			}
			catch (Db4oException e)
			{
				throw;
			}
			catch (Exception x)
			{
				throw new EventException(x);
			}
		}

		public virtual event System.EventHandler<Db4objects.Db4o.Events.ObjectContainerEventArgs>
			 Opened
		{
			add
			{
				_opened = (System.EventHandler<Db4objects.Db4o.Events.ObjectContainerEventArgs>)System.Delegate.Combine
					(_opened, value);
			}
			remove
			{
				_opened = (System.EventHandler<Db4objects.Db4o.Events.ObjectContainerEventArgs>)System.Delegate.Remove
					(_opened, value);
			}
		}

		public static bool InCallback(IInternalObjectContainer container)
		{
			if (container.Callbacks() is EventRegistryImpl)
			{
				EventRegistryImpl er = (EventRegistryImpl)container.Callbacks();
				return er._inCallback;
			}
			return false;
		}
	}
}
