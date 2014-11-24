/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Ids
{
	public class IdSlotChanges
	{
		private readonly LockedTree _slotChanges = new LockedTree();

		private readonly TransactionalIdSystemImpl _idSystem;

		private readonly IClosure4 _freespaceManager;

		private TreeInt _prefetchedIDs;

		public IdSlotChanges(TransactionalIdSystemImpl idSystem, IClosure4 freespaceManager
			)
		{
			_idSystem = idSystem;
			_freespaceManager = freespaceManager;
		}

		public void AccumulateFreeSlots(FreespaceCommitter freespaceCommitter, bool forFreespace
			, bool traverseMutable)
		{
			IVisitor4 visitor = new _IVisitor4_27(this, freespaceCommitter, forFreespace);
			if (traverseMutable)
			{
				_slotChanges.TraverseMutable(visitor);
			}
			else
			{
				_slotChanges.TraverseLocked(visitor);
			}
		}

		private sealed class _IVisitor4_27 : IVisitor4
		{
			public _IVisitor4_27(IdSlotChanges _enclosing, FreespaceCommitter freespaceCommitter
				, bool forFreespace)
			{
				this._enclosing = _enclosing;
				this.freespaceCommitter = freespaceCommitter;
				this.forFreespace = forFreespace;
			}

			public void Visit(object obj)
			{
				((SlotChange)obj).AccumulateFreeSlot(this._enclosing._idSystem, freespaceCommitter
					, forFreespace);
			}

			private readonly IdSlotChanges _enclosing;

			private readonly FreespaceCommitter freespaceCommitter;

			private readonly bool forFreespace;
		}

		public virtual void Clear()
		{
			_slotChanges.Clear();
		}

		public virtual void Rollback()
		{
			_slotChanges.TraverseLocked(new _IVisitor4_44(this));
		}

		private sealed class _IVisitor4_44 : IVisitor4
		{
			public _IVisitor4_44(IdSlotChanges _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object slotChange)
			{
				((SlotChange)slotChange).Rollback(this._enclosing.FreespaceManager());
			}

			private readonly IdSlotChanges _enclosing;
		}

		public virtual bool IsDeleted(int id)
		{
			SlotChange slot = FindSlotChange(id);
			if (slot == null)
			{
				return false;
			}
			return slot.IsDeleted();
		}

		public virtual SlotChange ProduceSlotChange(int id, SlotChangeFactory slotChangeFactory
			)
		{
			if (DTrace.enabled)
			{
				DTrace.ProduceSlotChange.Log(id);
			}
			SlotChange slot = slotChangeFactory.NewInstance(id);
			_slotChanges.Add(slot);
			return (SlotChange)slot.AddedOrExisting();
		}

		public SlotChange FindSlotChange(int id)
		{
			return (SlotChange)_slotChanges.Find(id);
		}

		public virtual void TraverseSlotChanges(IVisitor4 visitor)
		{
			_slotChanges.TraverseLocked(visitor);
		}

		public virtual bool IsDirty()
		{
			return !_slotChanges.IsEmpty();
		}

		public virtual void ReadSlotChanges(ByteArrayBuffer buffer)
		{
			_slotChanges.Read(buffer, new SlotChange(0));
		}

		public virtual void AddPrefetchedID(int id)
		{
			_prefetchedIDs = ((TreeInt)Tree.Add(_prefetchedIDs, new TreeInt(id)));
		}

		public virtual void PrefetchedIDConsumed(int id)
		{
			_prefetchedIDs = ((TreeInt)_prefetchedIDs.RemoveLike(new TreeInt(id)));
		}

		internal void FreePrefetchedIDs(IIdSystem idSystem)
		{
			if (_prefetchedIDs == null)
			{
				return;
			}
			idSystem.ReturnUnusedIds(_prefetchedIDs);
			_prefetchedIDs = null;
		}

		public virtual void NotifySlotCreated(int id, Slot slot, SlotChangeFactory slotChangeFactory
			)
		{
			ProduceSlotChange(id, slotChangeFactory).NotifySlotCreated(slot);
		}

		internal virtual void NotifySlotUpdated(int id, Slot slot, SlotChangeFactory slotChangeFactory
			)
		{
			ProduceSlotChange(id, slotChangeFactory).NotifySlotUpdated(FreespaceManager(), slot
				);
		}

		public virtual void NotifySlotDeleted(int id, SlotChangeFactory slotChangeFactory
			)
		{
			ProduceSlotChange(id, slotChangeFactory).NotifyDeleted(FreespaceManager());
		}

		private IFreespaceManager FreespaceManager()
		{
			return ((IFreespaceManager)_freespaceManager.Run());
		}
	}
}
