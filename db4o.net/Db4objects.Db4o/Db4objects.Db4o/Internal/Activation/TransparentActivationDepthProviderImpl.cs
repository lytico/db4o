/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Reflect.Generic;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Internal.Activation
{
	public class TransparentActivationDepthProviderImpl : IActivationDepthProvider, ITransparentActivationDepthProvider
	{
		public virtual IActivationDepth ActivationDepth(int depth, ActivationMode mode)
		{
			if (int.MaxValue == depth)
			{
				return new FullActivationDepth(mode);
			}
			return new FixedActivationDepth(depth, mode);
		}

		public virtual IActivationDepth ActivationDepthFor(ClassMetadata classMetadata, ActivationMode
			 mode)
		{
			if (IsTAAware(classMetadata))
			{
				return new NonDescendingActivationDepth(mode);
			}
			if (mode.IsPrefetch())
			{
				return new FixedActivationDepth(1, mode);
			}
			return new DescendingActivationDepth(this, mode);
		}

		private bool IsTAAware(ClassMetadata classMetadata)
		{
			GenericReflector reflector = classMetadata.Reflector();
			return reflector.ForClass(typeof(IActivatable)).IsAssignableFrom(classMetadata.ClassReflector
				());
		}

		private IRollbackStrategy _rollbackStrategy;

		public bool _transparentPersistenceIsEnabled;

		public virtual void EnableTransparentPersistenceSupportFor(IInternalObjectContainer
			 container, IRollbackStrategy rollbackStrategy)
		{
			FlushOnQueryStarted(container);
			_rollbackStrategy = rollbackStrategy;
			_transparentPersistenceIsEnabled = true;
		}

		private void FlushOnQueryStarted(IInternalObjectContainer container)
		{
			IEventRegistry registry = EventRegistryFactory.ForObjectContainer(container);
			registry.QueryStarted += new System.EventHandler<Db4objects.Db4o.Events.QueryEventArgs>
				(new _IEventListener4_46(this).OnEvent);
		}

		private sealed class _IEventListener4_46
		{
			public _IEventListener4_46(TransparentActivationDepthProviderImpl _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.QueryEventArgs args)
			{
				this._enclosing.ObjectsModifiedIn(this._enclosing.TransactionFrom(args)).Flush();
			}

			private readonly TransparentActivationDepthProviderImpl _enclosing;
		}

		protected virtual Transaction TransactionFrom(EventArgs args)
		{
			return (Transaction)((TransactionalEventArgs)args).Transaction();
		}

		public virtual void AddModified(object @object, Transaction transaction)
		{
			if (!_transparentPersistenceIsEnabled)
			{
				return;
			}
			ObjectsModifiedIn(transaction).Add(@object);
		}

		public virtual void RemoveModified(object @object, Transaction transaction)
		{
			if (!_transparentPersistenceIsEnabled)
			{
				return;
			}
			ObjectsModifiedIn(transaction).Remove(@object);
		}

		private sealed class _TransactionLocal_73 : TransactionLocal
		{
			public _TransactionLocal_73(TransparentActivationDepthProviderImpl _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public override object InitialValueFor(Transaction transaction)
			{
				TransparentActivationDepthProviderImpl.ObjectsModifiedInTransaction objectsModifiedInTransaction
					 = new TransparentActivationDepthProviderImpl.ObjectsModifiedInTransaction(transaction
					);
				transaction.AddTransactionListener(new _ITransactionListener_77(this, objectsModifiedInTransaction
					));
				return objectsModifiedInTransaction;
			}

			private sealed class _ITransactionListener_77 : ITransactionListener
			{
				public _ITransactionListener_77(_TransactionLocal_73 _enclosing, TransparentActivationDepthProviderImpl.ObjectsModifiedInTransaction
					 objectsModifiedInTransaction)
				{
					this._enclosing = _enclosing;
					this.objectsModifiedInTransaction = objectsModifiedInTransaction;
				}

				public void PostRollback()
				{
					objectsModifiedInTransaction.Rollback(this._enclosing._enclosing._rollbackStrategy
						);
				}

				public void PreCommit()
				{
					objectsModifiedInTransaction.Flush();
				}

				private readonly _TransactionLocal_73 _enclosing;

				private readonly TransparentActivationDepthProviderImpl.ObjectsModifiedInTransaction
					 objectsModifiedInTransaction;
			}

			private readonly TransparentActivationDepthProviderImpl _enclosing;
		}

		private readonly TransactionLocal _objectsModifiedInTransaction;

		private TransparentActivationDepthProviderImpl.ObjectsModifiedInTransaction ObjectsModifiedIn
			(Transaction transaction)
		{
			return ((TransparentActivationDepthProviderImpl.ObjectsModifiedInTransaction)transaction
				.Get(_objectsModifiedInTransaction).value);
		}

		private sealed class ObjectsModifiedInTransaction
		{
			private readonly IdentitySet4 _removedAfterModified = new IdentitySet4();

			private readonly IdentitySet4 _modified = new IdentitySet4();

			private readonly Transaction _transaction;

			public ObjectsModifiedInTransaction(Transaction transaction)
			{
				_transaction = transaction;
			}

			public void Add(object @object)
			{
				if (Contains(@object))
				{
					return;
				}
				_modified.Add(@object);
			}

			public void Remove(object @object)
			{
				if (!Contains(@object))
				{
					return;
				}
				_modified.Remove(@object);
				_removedAfterModified.Add(@object);
			}

			private bool Contains(object @object)
			{
				return _modified.Contains(@object);
			}

			public void Flush()
			{
				StoreModifiedObjects();
				_modified.Clear();
			}

			private void StoreModifiedObjects()
			{
				ObjectContainerBase container = _transaction.Container();
				container.StoreAll(_transaction, _modified.ValuesIterator(), container.UpdateDepthProvider
					().Unspecified(new _IModifiedObjectQuery_132(this)));
				_transaction.ProcessDeletes();
			}

			private sealed class _IModifiedObjectQuery_132 : IModifiedObjectQuery
			{
				public _IModifiedObjectQuery_132(ObjectsModifiedInTransaction _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public bool IsModified(object obj)
				{
					return this._enclosing.Contains(obj);
				}

				private readonly ObjectsModifiedInTransaction _enclosing;
			}

			public void Rollback(IRollbackStrategy rollbackStrategy)
			{
				ApplyRollbackStrategy(rollbackStrategy);
				_modified.Clear();
			}

			private void ApplyRollbackStrategy(IRollbackStrategy rollbackStrategy)
			{
				if (null == rollbackStrategy)
				{
					return;
				}
				ApplyRollbackStrategy(rollbackStrategy, _modified.ValuesIterator());
				ApplyRollbackStrategy(rollbackStrategy, _removedAfterModified.ValuesIterator());
			}

			private void ApplyRollbackStrategy(IRollbackStrategy rollbackStrategy, IEnumerator
				 values)
			{
				IObjectContainer objectContainer = _transaction.ObjectContainer();
				while (values.MoveNext())
				{
					rollbackStrategy.Rollback(objectContainer, values.Current);
				}
			}
		}

		public TransparentActivationDepthProviderImpl()
		{
			_objectsModifiedInTransaction = new _TransactionLocal_73(this);
		}
	}
}
