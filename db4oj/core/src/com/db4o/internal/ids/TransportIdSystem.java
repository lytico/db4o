/* Copyright (C) 2009 Versant Corporation http://www.db4o.com */

package com.db4o.internal.ids;

import com.db4o.internal.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public final class TransportIdSystem implements TransactionalIdSystem {
	
	private final LocalObjectContainer _container;
	
	public TransportIdSystem(LocalObjectContainer localObjectContainer) {
		_container = localObjectContainer;
	}
	
	public int newId(SlotChangeFactory slotChangeFactory) {
		return _container.allocatePointerSlot();
	}
	
	public void notifySlotCreated(int id, Slot slot,
			SlotChangeFactory slotChangeFactory) {
		writePointer(id, slot);
	}

	private void writePointer(int id, Slot slot) {
		_container.writePointer(id, slot);
	}
	
	public void notifySlotUpdated(int id, Slot slot, SlotChangeFactory slotChangeFactory) {
		writePointer(id, slot);
	}
	
	public void notifySlotDeleted(int id, SlotChangeFactory slotChangeFactory) {
		writePointer(id, Slot.ZERO);
	}
	
	public void commit(FreespaceCommitter accumulator) {
		// don't do anything
	}
	
	public Slot currentSlot(int id) {
		return committedSlot(id); 
	}
	
	public void collectCallBackInfo(CallbackInfoCollector collector) {
		// do nothing
	}
	
	public void clear() {
		// TODO Auto-generated method stub
		
	}

	public Slot committedSlot(int id) {
		return _container.readPointerSlot(id);
	}

	public boolean isDeleted(int id) {
		return false;
	}

	public boolean isDirty() {
		return false;
	}

	public int prefetchID() {
		return 0;
	}

	public void prefetchedIDConsumed(int id) {
		
	}

	public void rollback() {
		
	}

	public void close() {
		
	}

	public void accumulateFreeSlots(FreespaceCommitter freespaceCommitter, boolean forFreespace) {
		
	}

}
