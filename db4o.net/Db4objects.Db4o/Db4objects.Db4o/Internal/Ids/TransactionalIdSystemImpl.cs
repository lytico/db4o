/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public class TransactionalIdSystemImpl : ITransactionalIdSystem
	{
		private IdSlotChanges _slotChanges;

		private Db4objects.Db4o.Internal.Ids.TransactionalIdSystemImpl _parentIdSystem;

		private readonly IClosure4 _globalIdSystem;

		public TransactionalIdSystemImpl(IClosure4 freespaceManager, IClosure4 globalIdSystem
			, Db4objects.Db4o.Internal.Ids.TransactionalIdSystemImpl parentIdSystem)
		{
			_globalIdSystem = globalIdSystem;
			_slotChanges = new IdSlotChanges(this, freespaceManager);
			_parentIdSystem = parentIdSystem;
		}

		public virtual void CollectCallBackInfo(ICallbackInfoCollector collector)
		{
			if (!_slotChanges.IsDirty())
			{
				return;
			}
			_slotChanges.TraverseSlotChanges(new _IVisitor4_31(collector));
		}

		private sealed class _IVisitor4_31 : IVisitor4
		{
			public _IVisitor4_31(ICallbackInfoCollector collector)
			{
				this.collector = collector;
			}

			public void Visit(object slotChange)
			{
				int id = ((TreeInt)slotChange)._key;
				if (((SlotChange)slotChange).IsDeleted())
				{
					if (!((SlotChange)slotChange).IsNew())
					{
						collector.Deleted(id);
					}
				}
				else
				{
					if (((SlotChange)slotChange).IsNew())
					{
						collector.Added(id);
					}
					else
					{
						collector.Updated(id);
					}
				}
			}

			private readonly ICallbackInfoCollector collector;
		}

		public virtual bool IsDirty()
		{
			return _slotChanges.IsDirty();
		}

		public virtual void Commit(FreespaceCommitter freespaceCommitter)
		{
			IVisitable slotChangeVisitable = new _IVisitable_52(this);
			freespaceCommitter.TransactionalIdSystem(this);
			AccumulateFreeSlots(freespaceCommitter, false);
			GlobalIdSystem().Commit(slotChangeVisitable, freespaceCommitter);
		}

		private sealed class _IVisitable_52 : IVisitable
		{
			public _IVisitable_52(TransactionalIdSystemImpl _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Accept(IVisitor4 visitor)
			{
				this._enclosing.TraverseSlotChanges(visitor);
			}

			private readonly TransactionalIdSystemImpl _enclosing;
		}

		public virtual void AccumulateFreeSlots(FreespaceCommitter accumulator, bool forFreespace
			)
		{
			_slotChanges.AccumulateFreeSlots(accumulator, forFreespace, IsSystemIdSystem());
			if (_parentIdSystem != null)
			{
				_parentIdSystem.AccumulateFreeSlots(accumulator, forFreespace);
			}
		}

		private bool IsSystemIdSystem()
		{
			return _parentIdSystem == null;
		}

		public virtual void CompleteInterruptedTransaction(int transactionId1, int transactionId2
			)
		{
			GlobalIdSystem().CompleteInterruptedTransaction(transactionId1, transactionId2);
		}

		public virtual Slot CommittedSlot(int id)
		{
			if (id == 0)
			{
				return null;
			}
			return GlobalIdSystem().CommittedSlot(id);
		}

		public virtual Slot CurrentSlot(int id)
		{
			Slot slot = ModifiedSlot(id);
			if (slot != null)
			{
				return slot;
			}
			return CommittedSlot(id);
		}

		public virtual Slot ModifiedSlot(int id)
		{
			if (id == 0)
			{
				return null;
			}
			SlotChange change = _slotChanges.FindSlotChange(id);
			if (change != null)
			{
				if (change.SlotModified())
				{
					return change.NewSlot();
				}
			}
			return ModifiedSlotInParentIdSystem(id);
		}

		public Slot ModifiedSlotInParentIdSystem(int id)
		{
			if (_parentIdSystem == null)
			{
				return null;
			}
			return _parentIdSystem.ModifiedSlot(id);
		}

		public virtual void Rollback()
		{
			_slotChanges.Rollback();
		}

		public virtual void Clear()
		{
			_slotChanges.Clear();
		}

		public virtual bool IsDeleted(int id)
		{
			return _slotChanges.IsDeleted(id);
		}

		public virtual void NotifySlotUpdated(int id, Slot slot, SlotChangeFactory slotChangeFactory
			)
		{
			_slotChanges.NotifySlotUpdated(id, slot, slotChangeFactory);
		}

		private void TraverseSlotChanges(IVisitor4 visitor)
		{
			if (_parentIdSystem != null)
			{
				_parentIdSystem.TraverseSlotChanges(visitor);
			}
			_slotChanges.TraverseSlotChanges(visitor);
		}

		public virtual int NewId(SlotChangeFactory slotChangeFactory)
		{
			int id = AcquireId();
			_slotChanges.ProduceSlotChange(id, slotChangeFactory).NotifySlotCreated(null);
			return id;
		}

		private int AcquireId()
		{
			return GlobalIdSystem().NewId();
		}

		public virtual int PrefetchID()
		{
			int id = AcquireId();
			_slotChanges.AddPrefetchedID(id);
			return id;
		}

		public virtual void PrefetchedIDConsumed(int id)
		{
			_slotChanges.PrefetchedIDConsumed(id);
		}

		public virtual void NotifySlotCreated(int id, Slot slot, SlotChangeFactory slotChangeFactory
			)
		{
			_slotChanges.NotifySlotCreated(id, slot, slotChangeFactory);
		}

		public virtual void NotifySlotDeleted(int id, SlotChangeFactory slotChangeFactory
			)
		{
			_slotChanges.NotifySlotDeleted(id, slotChangeFactory);
		}

		private IIdSystem GlobalIdSystem()
		{
			return ((IIdSystem)_globalIdSystem.Run());
		}

		public virtual void Close()
		{
			_slotChanges.FreePrefetchedIDs(GlobalIdSystem());
		}
	}
}
