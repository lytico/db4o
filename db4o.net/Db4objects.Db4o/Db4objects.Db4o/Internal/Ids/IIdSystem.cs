/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public interface IIdSystem
	{
		int NewId();

		Slot CommittedSlot(int id);

		void ReturnUnusedIds(IVisitable visitable);

		void Close();

		void CompleteInterruptedTransaction(int transactionId1, int transactionId2);

		void Commit(IVisitable slotChanges, FreespaceCommitter freespaceCommitter);

		void TraverseOwnSlots(IProcedure4 block);
	}
}
