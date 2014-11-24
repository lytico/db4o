/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Slots
{
	/// <exclude></exclude>
	public class SlotChange : TreeInt
	{
		private class SlotChangeOperation
		{
			private readonly string _type;

			public SlotChangeOperation(string type)
			{
				_type = type;
			}

			internal static readonly SlotChange.SlotChangeOperation create = new SlotChange.SlotChangeOperation
				("create");

			internal static readonly SlotChange.SlotChangeOperation update = new SlotChange.SlotChangeOperation
				("update");

			internal static readonly SlotChange.SlotChangeOperation delete = new SlotChange.SlotChangeOperation
				("delete");

			public override string ToString()
			{
				return _type;
			}
		}

		private SlotChange.SlotChangeOperation _firstOperation;

		private SlotChange.SlotChangeOperation _currentOperation;

		protected Slot _newSlot;

		public SlotChange(int id) : base(id)
		{
		}

		public override object ShallowClone()
		{
			SlotChange sc = new SlotChange(0);
			sc.NewSlot(_newSlot);
			return base.ShallowCloneInternal(sc);
		}

		public virtual void AccumulateFreeSlot(TransactionalIdSystemImpl idSystem, FreespaceCommitter
			 freespaceCommitter, bool forFreespace)
		{
			if (ForFreespace() != forFreespace)
			{
				return;
			}
			if (_firstOperation == SlotChange.SlotChangeOperation.create)
			{
				return;
			}
			if (_currentOperation == SlotChange.SlotChangeOperation.update || _currentOperation
				 == SlotChange.SlotChangeOperation.delete)
			{
				Slot slot = ModifiedSlotInParentIdSystem(idSystem);
				if (Slot.IsNull(slot))
				{
					slot = idSystem.CommittedSlot(_key);
				}
				// No old slot at all can be the case if the object
				// has been deleted by another transaction and we add it again.
				if (!Slot.IsNull(slot))
				{
					freespaceCommitter.DelayedFree(slot, FreeToSystemFreespaceSystem());
				}
			}
		}

		protected virtual bool ForFreespace()
		{
			return false;
		}

		protected virtual Slot ModifiedSlotInParentIdSystem(TransactionalIdSystemImpl idSystem
			)
		{
			return idSystem.ModifiedSlotInParentIdSystem(_key);
		}

		public virtual bool IsDeleted()
		{
			return SlotModified() && _newSlot.IsNull();
		}

		public virtual bool IsNew()
		{
			return _firstOperation == SlotChange.SlotChangeOperation.create;
		}

		private bool IsFreeOnRollback()
		{
			return !Slot.IsNull(_newSlot);
		}

		public bool SlotModified()
		{
			return _newSlot != null;
		}

		/// <summary>FIXME:	Check where pointers should be freed on commit.</summary>
		/// <remarks>
		/// FIXME:	Check where pointers should be freed on commit.
		/// This should be triggered in this class.
		/// </remarks>
		public virtual Slot NewSlot()
		{
			//	private final boolean isFreePointerOnCommit() {
			//		return isBitSet(FREE_POINTER_ON_COMMIT_BIT);
			//	}
			return _newSlot;
		}

		public override object Read(ByteArrayBuffer reader)
		{
			SlotChange change = new SlotChange(reader.ReadInt());
			Slot newSlot = new Slot(reader.ReadInt(), reader.ReadInt());
			change.NewSlot(newSlot);
			return change;
		}

		public virtual void Rollback(IFreespaceManager freespaceManager)
		{
			if (IsFreeOnRollback())
			{
				freespaceManager.Free(_newSlot);
			}
		}

		public override void Write(ByteArrayBuffer writer)
		{
			if (SlotModified())
			{
				writer.WriteInt(_key);
				writer.WriteInt(_newSlot.Address());
				writer.WriteInt(_newSlot.Length());
			}
		}

		public void WritePointer(LocalObjectContainer container)
		{
			if (SlotModified())
			{
				container.WritePointer(_key, _newSlot);
			}
		}

		private void NewSlot(Slot slot)
		{
			_newSlot = slot;
		}

		public virtual void NotifySlotUpdated(IFreespaceManager freespaceManager, Slot slot
			)
		{
			if (DTrace.enabled)
			{
				DTrace.NotifySlotUpdated.LogLength(_key, slot);
			}
			FreePreviouslyModifiedSlot(freespaceManager);
			_newSlot = slot;
			Operation(SlotChange.SlotChangeOperation.update);
		}

		protected virtual void FreePreviouslyModifiedSlot(IFreespaceManager freespaceManager
			)
		{
			if (Slot.IsNull(_newSlot))
			{
				return;
			}
			Free(freespaceManager, _newSlot);
			_newSlot = null;
		}

		protected virtual void Free(IFreespaceManager freespaceManager, Slot slot)
		{
			if (slot.IsNull())
			{
				return;
			}
			if (freespaceManager == null)
			{
				return;
			}
			freespaceManager.Free(slot);
		}

		private void Operation(SlotChange.SlotChangeOperation operation)
		{
			if (_firstOperation == null)
			{
				_firstOperation = operation;
			}
			_currentOperation = operation;
		}

		public virtual void NotifySlotCreated(Slot slot)
		{
			if (DTrace.enabled)
			{
				DTrace.NotifySlotCreated.Log(_key);
				DTrace.NotifySlotCreated.LogLength(slot);
			}
			Operation(SlotChange.SlotChangeOperation.create);
			_newSlot = slot;
		}

		public virtual void NotifyDeleted(IFreespaceManager freespaceManager)
		{
			if (DTrace.enabled)
			{
				DTrace.NotifySlotDeleted.Log(_key);
			}
			Operation(SlotChange.SlotChangeOperation.delete);
			FreePreviouslyModifiedSlot(freespaceManager);
			_newSlot = Slot.Zero;
		}

		public virtual bool RemoveId()
		{
			return false;
		}

		public override string ToString()
		{
			string str = "id: " + _key;
			if (_newSlot != null)
			{
				str += " newSlot: " + _newSlot;
			}
			return str;
		}

		protected virtual bool FreeToSystemFreespaceSystem()
		{
			return false;
		}
	}
}
