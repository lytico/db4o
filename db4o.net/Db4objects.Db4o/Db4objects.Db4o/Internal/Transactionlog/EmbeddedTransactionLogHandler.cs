/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Internal.Transactionlog;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Transactionlog
{
	/// <exclude></exclude>
	public class EmbeddedTransactionLogHandler : TransactionLogHandler
	{
		public EmbeddedTransactionLogHandler(LocalObjectContainer container) : base(container
			)
		{
		}

		public override void CompleteInterruptedTransaction(int transactionId1, int transactionId2
			)
		{
			if (transactionId1 <= 0 || transactionId1 != transactionId2)
			{
				return;
			}
			StatefulBuffer bytes = new StatefulBuffer(_container.SystemTransaction(), transactionId1
				, Const4.IntLength);
			bytes.Read();
			int length = bytes.ReadInt();
			if (length > 0)
			{
				bytes = new StatefulBuffer(_container.SystemTransaction(), transactionId1, length
					);
				bytes.Read();
				bytes.IncrementOffset(Const4.IntLength);
				ReadWriteSlotChanges(bytes);
			}
			_container.WriteTransactionPointer(0);
			FlushDatabaseFile();
		}

		public override Slot AllocateSlot(bool appendToFile, int slotChangeCount)
		{
			int transactionLogByteCount = TransactionLogSlotLength(slotChangeCount);
			IFreespaceManager freespaceManager = _container.FreespaceManager();
			if (!appendToFile && freespaceManager != null)
			{
				Slot slot = freespaceManager.AllocateTransactionLogSlot(transactionLogByteCount);
				if (slot != null)
				{
					return slot;
				}
			}
			return _container.AppendBytes(transactionLogByteCount);
		}

		private void FreeSlot(Slot slot)
		{
			if (slot == null)
			{
				return;
			}
			if (_container.FreespaceManager() == null)
			{
				return;
			}
			_container.FreespaceManager().FreeSafeSlot(slot);
		}

		public override void ApplySlotChanges(IVisitable slotChangeTree, int slotChangeCount
			, Slot reservedSlot)
		{
			if (slotChangeCount > 0)
			{
				Slot transactionLogSlot = SlotLongEnoughForLog(slotChangeCount, reservedSlot) ? reservedSlot
					 : AllocateSlot(true, slotChangeCount);
				StatefulBuffer buffer = new StatefulBuffer(_container.SystemTransaction(), transactionLogSlot
					);
				buffer.WriteInt(transactionLogSlot.Length());
				buffer.WriteInt(slotChangeCount);
				AppendSlotChanges(buffer, slotChangeTree);
				buffer.Write();
				IRunnable commitHook = _container.CommitHook();
				FlushDatabaseFile();
				_container.WriteTransactionPointer(transactionLogSlot.Address());
				FlushDatabaseFile();
				if (WriteSlots(slotChangeTree))
				{
					FlushDatabaseFile();
				}
				_container.WriteTransactionPointer(0);
				commitHook.Run();
				FlushDatabaseFile();
				if (transactionLogSlot != reservedSlot)
				{
					FreeSlot(transactionLogSlot);
				}
			}
			FreeSlot(reservedSlot);
		}

		private bool SlotLongEnoughForLog(int slotChangeCount, Slot slot)
		{
			return slot != null && slot.Length() >= TransactionLogSlotLength(slotChangeCount);
		}

		public override void Close()
		{
		}
		// do nothing
	}
}
