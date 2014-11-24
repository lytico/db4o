/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.References;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class Transaction
	{
		private IContext _context;

		internal Tree _delete;

		protected readonly Db4objects.Db4o.Internal.Transaction _systemTransaction;

		/// <summary>
		/// This is the inside representation to operate against, the actual
		/// file-based ObjectContainerBase or the client.
		/// </summary>
		/// <remarks>
		/// This is the inside representation to operate against, the actual
		/// file-based ObjectContainerBase or the client. For all calls
		/// against this ObjectContainerBase the method signatures that take
		/// a transaction have to be used.
		/// </remarks>
		private readonly ObjectContainerBase _container;

		/// <summary>This is the outside representation to the user.</summary>
		/// <remarks>
		/// This is the outside representation to the user. This ObjectContainer
		/// should use this transaction as it's main user transation, so it also
		/// allows using the method signatures on ObjectContainer without a
		/// transaction.
		/// </remarks>
		private IObjectContainer _objectContainer;

		private List4 _transactionListeners;

		private readonly IReferenceSystem _referenceSystem;

		private readonly IDictionary _locals = new Hashtable();

		public Transaction(ObjectContainerBase container, Db4objects.Db4o.Internal.Transaction
			 systemTransaction, IReferenceSystem referenceSystem)
		{
			// contains DeleteInfo nodes
			_container = container;
			_systemTransaction = systemTransaction;
			_referenceSystem = referenceSystem;
		}

		/// <summary>Retrieves the value of a transaction local variables.</summary>
		/// <remarks>
		/// Retrieves the value of a transaction local variables.
		/// If this is the first time the variable is accessed
		/// <see cref="TransactionLocal.InitialValueFor(Transaction)">TransactionLocal.InitialValueFor(Transaction)
		/// 	</see>
		/// will provide the initial value.
		/// </remarks>
		public virtual ByRef Get(TransactionLocal local)
		{
			ByRef existing = (ByRef)_locals[local];
			if (null != existing)
			{
				return existing;
			}
			ByRef initialValue = ByRef.NewInstance(local.InitialValueFor(this));
			_locals[local] = initialValue;
			return initialValue;
		}

		public void CheckSynchronization()
		{
		}

		public virtual void AddTransactionListener(ITransactionListener listener)
		{
			_transactionListeners = new List4(_transactionListeners, listener);
		}

		protected void ClearAll()
		{
			Clear();
			_transactionListeners = null;
			_locals.Clear();
		}

		protected abstract void Clear();

		public virtual void Close(bool rollbackOnClose)
		{
			if (Container() != null)
			{
				CheckSynchronization();
				Container().ReleaseSemaphores(this);
				DiscardReferenceSystem();
			}
			if (rollbackOnClose)
			{
				Rollback();
			}
			ITransactionalIdSystem idSystem = IdSystem();
			if (idSystem != null)
			{
				idSystem.Close();
			}
		}

		protected virtual void DiscardReferenceSystem()
		{
			if (_referenceSystem != null)
			{
				Container().ReferenceSystemRegistry().RemoveReferenceSystem(_referenceSystem);
			}
		}

		public abstract void Commit();

		protected virtual void CommitTransactionListeners()
		{
			CheckSynchronization();
			if (_transactionListeners != null)
			{
				IEnumerator i = new Iterator4Impl(_transactionListeners);
				while (i.MoveNext())
				{
					((ITransactionListener)i.Current).PreCommit();
				}
				_transactionListeners = null;
			}
		}

		protected virtual bool IsSystemTransaction()
		{
			return _systemTransaction == null;
		}

		public virtual bool Delete(ObjectReference @ref, int id, int cascade)
		{
			CheckSynchronization();
			if (@ref != null)
			{
				if (!_container.FlagForDelete(@ref))
				{
					return false;
				}
			}
			if (DTrace.enabled)
			{
				DTrace.TransDelete.Log(id);
			}
			DeleteInfo info = (DeleteInfo)TreeInt.Find(_delete, id);
			if (info == null)
			{
				info = new DeleteInfo(id, @ref, cascade);
				_delete = Tree.Add(_delete, info);
				return true;
			}
			info._reference = @ref;
			if (cascade > info._cascade)
			{
				info._cascade = cascade;
			}
			return true;
		}

		public virtual void DontDelete(int a_id)
		{
			if (DTrace.enabled)
			{
				DTrace.TransDontDelete.Log(a_id);
			}
			if (_delete == null)
			{
				return;
			}
			_delete = TreeInt.RemoveLike((TreeInt)_delete, a_id);
		}

		public abstract void ProcessDeletes();

		public virtual IReferenceSystem ReferenceSystem()
		{
			if (_referenceSystem != null)
			{
				return _referenceSystem;
			}
			return ParentTransaction().ReferenceSystem();
		}

		public IReflector Reflector()
		{
			return Container().Reflector();
		}

		public abstract void Rollback();

		protected virtual void RollBackTransactionListeners()
		{
			CheckSynchronization();
			if (_transactionListeners != null)
			{
				IEnumerator i = new Iterator4Impl(_transactionListeners);
				while (i.MoveNext())
				{
					((ITransactionListener)i.Current).PostRollback();
				}
				_transactionListeners = null;
			}
		}

		internal virtual bool SupportsVirtualFields()
		{
			return true;
		}

		public virtual Db4objects.Db4o.Internal.Transaction SystemTransaction()
		{
			if (_systemTransaction != null)
			{
				return _systemTransaction;
			}
			return this;
		}

		public override string ToString()
		{
			return Container().ToString();
		}

		public abstract void WriteUpdateAdjustIndexes(int id, ClassMetadata clazz, ArrayType
			 typeInfo);

		public ObjectContainerBase Container()
		{
			return _container;
		}

		public virtual Db4objects.Db4o.Internal.Transaction ParentTransaction()
		{
			return _systemTransaction;
		}

		public virtual void RollbackReferenceSystem()
		{
			ReferenceSystem().Rollback();
		}

		public virtual void PostCommit()
		{
			CommitReferenceSystem();
		}

		public virtual void CommitReferenceSystem()
		{
			ReferenceSystem().Commit();
		}

		public virtual void AddNewReference(ObjectReference @ref)
		{
			ReferenceSystem().AddNewReference(@ref);
		}

		public object ObjectForIdFromCache(int id)
		{
			ObjectReference @ref = ReferenceForId(id);
			if (@ref == null)
			{
				return null;
			}
			object candidate = @ref.GetObject();
			if (candidate == null)
			{
				RemoveReference(@ref);
			}
			return candidate;
		}

		public ObjectReference ReferenceForId(int id)
		{
			ObjectReference @ref = ReferenceSystem().ReferenceForId(id);
			if (@ref != null)
			{
				if (@ref.GetObject() == null)
				{
					RemoveReference(@ref);
					return null;
				}
				return @ref;
			}
			if (ParentTransaction() != null)
			{
				return ParentTransaction().ReferenceForId(id);
			}
			return null;
		}

		public ObjectReference ReferenceForObject(object obj)
		{
			ObjectReference @ref = ReferenceSystem().ReferenceForObject(obj);
			if (@ref != null)
			{
				return @ref;
			}
			if (ParentTransaction() != null)
			{
				return ParentTransaction().ReferenceForObject(obj);
			}
			return null;
		}

		public void RemoveReference(ObjectReference @ref)
		{
			ReferenceSystem().RemoveReference(@ref);
			// setting the ID to minus 1 ensures that the
			// gc mechanism does not kill the new YapObject
			@ref.SetID(-1);
			Platform4.KillYapRef(@ref.GetObjectReference());
		}

		public void RemoveObjectFromReferenceSystem(object obj)
		{
			ObjectReference @ref = ReferenceForObject(obj);
			if (@ref != null)
			{
				RemoveReference(@ref);
			}
		}

		public virtual void SetOutSideRepresentation(IObjectContainer objectContainer)
		{
			_objectContainer = objectContainer;
		}

		public virtual IObjectContainer ObjectContainer()
		{
			if (_objectContainer != null)
			{
				return _objectContainer;
			}
			return _container;
		}

		public virtual IContext Context()
		{
			if (_context == null)
			{
				_context = new _IContext_299(this);
			}
			return _context;
		}

		private sealed class _IContext_299 : IContext
		{
			public _IContext_299(Transaction _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public IObjectContainer ObjectContainer()
			{
				return this._enclosing.ObjectContainer();
			}

			public Db4objects.Db4o.Internal.Transaction Transaction()
			{
				return this._enclosing;
			}

			private readonly Transaction _enclosing;
		}

		protected virtual void TraverseDelete(IVisitor4 deleteVisitor)
		{
			if (_delete == null)
			{
				return;
			}
			_delete.Traverse(deleteVisitor);
			_delete = null;
		}

		public virtual object Wrap(object value)
		{
			if (value is int)
			{
				return value;
			}
			return new TransactionContext(this, value);
		}

		public abstract ITransactionalIdSystem IdSystem();

		public abstract long VersionForId(int id);

		public abstract long GenerateTransactionTimestamp(long forcedTimeStamp);

		public abstract void UseDefaultTransactionTimestamp();

		public virtual void PostOpen()
		{
			if (_systemTransaction != null)
			{
				_systemTransaction.PostOpen();
			}
		}
	}
}
