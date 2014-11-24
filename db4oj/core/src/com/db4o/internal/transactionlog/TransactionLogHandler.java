/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.transactionlog;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.slots.*;


/**
 * @exclude
 */
public abstract class TransactionLogHandler {
	
	protected final LocalObjectContainer _container;
	
	protected TransactionLogHandler(LocalObjectContainer container){
		_container = container;
	}
	
	protected LocalObjectContainer localContainer() {
		return _container;
	}
	
    protected final void flushDatabaseFile() {
		_container.syncFiles();
	}
    
	protected final void appendSlotChanges(final ByteArrayBuffer writer, Visitable slotChangeVisitable){
		slotChangeVisitable.accept(new Visitor4() {
			public void visit(Object obj) {
				((SlotChange)obj).write(writer);
			}
		});
    }
	
    protected boolean writeSlots(Visitable<SlotChange> slotChangeTree) {
        final BooleanByRef ret = new BooleanByRef();
        slotChangeTree.accept(new Visitor4() {
			public void visit(Object obj) {
				((SlotChange)obj).writePointer(_container);
				ret.value = true;
			}
		});
        return ret.value;
    }
    
	protected final int transactionLogSlotLength(int slotChangeCount){
    	// slotchanges * 3 for ID, address, length
    	// 2 ints for slotlength and count
    	return ((slotChangeCount * 3) + 2) * Const4.INT_LENGTH;
    }

	public abstract Slot allocateSlot(boolean append, int slotChangeCount);

	public abstract void applySlotChanges(Visitable<SlotChange> slotChangeTree, int slotChangeCount, Slot reservedSlot);

	public abstract void completeInterruptedTransaction(int transactionId1, int transactionId2);

	public abstract void close();
	
	protected void readWriteSlotChanges(ByteArrayBuffer buffer) {
		final LockedTree slotChanges = new LockedTree();
		slotChanges.read(buffer, new SlotChange(0));
		if(writeSlots(new Visitable<SlotChange>() {
			public void accept(Visitor4<SlotChange> visitor) {
				slotChanges.traverseMutable(visitor);
			}
		})){
			flushDatabaseFile();
		}
	}
	

}
