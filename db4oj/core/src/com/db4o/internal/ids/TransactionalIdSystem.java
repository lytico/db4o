/* Copyright (C) 2009 Versant Corporation http://www.db4o.com */

package com.db4o.internal.ids;

import com.db4o.internal.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public interface TransactionalIdSystem {

	public void collectCallBackInfo(CallbackInfoCollector collector);

	public boolean isDirty();

	public void commit(FreespaceCommitter freespaceCommitter);

	public Slot committedSlot(int id);

	public Slot currentSlot(int id);
	
	public void accumulateFreeSlots(FreespaceCommitter freespaceCommitter, boolean forFreespace);

	public void rollback();

	public void clear();

	public boolean isDeleted(int id);

	public void notifySlotUpdated(int id, Slot slot, SlotChangeFactory slotChangeFactory);
	
	public void notifySlotCreated(int id, Slot slot, SlotChangeFactory slotChangeFactory);
	
	public void notifySlotDeleted(int id, SlotChangeFactory slotChangeFactory);

	public int newId(SlotChangeFactory slotChangeFactory);

	public int prefetchID();

	public void prefetchedIDConsumed(int id);

	public void close();

}