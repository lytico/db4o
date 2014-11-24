/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public interface ITransactionalIdSystem
	{
		void CollectCallBackInfo(ICallbackInfoCollector collector);

		bool IsDirty();

		void Commit(FreespaceCommitter freespaceCommitter);

		Slot CommittedSlot(int id);

		Slot CurrentSlot(int id);

		void AccumulateFreeSlots(FreespaceCommitter freespaceCommitter, bool forFreespace
			);

		void Rollback();

		void Clear();

		bool IsDeleted(int id);

		void NotifySlotUpdated(int id, Slot slot, SlotChangeFactory slotChangeFactory);

		void NotifySlotCreated(int id, Slot slot, SlotChangeFactory slotChangeFactory);

		void NotifySlotDeleted(int id, SlotChangeFactory slotChangeFactory);

		int NewId(SlotChangeFactory slotChangeFactory);

		int PrefetchID();

		void PrefetchedIDConsumed(int id);

		void Close();
	}
}
