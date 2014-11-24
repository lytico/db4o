/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.ids;

import com.db4o.foundation.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public interface IdSystem {

	public int newId();

	public Slot committedSlot(int id);

	public void returnUnusedIds(Visitable<Integer> visitable);

	public void close();

	public void completeInterruptedTransaction(int transactionId1, int transactionId2);

	public void commit(Visitable<SlotChange> slotChanges, FreespaceCommitter freespaceCommitter);

	public void traverseOwnSlots(Procedure4<Pair<Integer, Slot>> block);
}
