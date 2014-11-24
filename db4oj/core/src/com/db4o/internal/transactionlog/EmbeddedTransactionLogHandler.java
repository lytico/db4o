/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.transactionlog;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.freespace.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public class EmbeddedTransactionLogHandler extends TransactionLogHandler{
	
	public EmbeddedTransactionLogHandler(LocalObjectContainer container) {
		super(container);
	}

	public void completeInterruptedTransaction(final int transactionId1, final int transactionId2) {
		if(transactionId1 <= 0 || transactionId1 != transactionId2){
			return;
		}
		StatefulBuffer bytes = new StatefulBuffer(_container.systemTransaction(), transactionId1, Const4.INT_LENGTH);
		bytes.read();
        int length = bytes.readInt();
        if (length > 0) {
            bytes = new StatefulBuffer(_container.systemTransaction(), transactionId1, length);
            bytes.read();
            bytes.incrementOffset(Const4.INT_LENGTH);
            readWriteSlotChanges(bytes);
        }
        _container.writeTransactionPointer(0);
        flushDatabaseFile();
	}

	public Slot allocateSlot(boolean appendToFile, int slotChangeCount) {
		int transactionLogByteCount = transactionLogSlotLength(slotChangeCount);
    	FreespaceManager freespaceManager = _container.freespaceManager();
		if(! appendToFile && freespaceManager != null){
    		Slot slot = freespaceManager.allocateTransactionLogSlot(transactionLogByteCount);
    		if(slot != null){
    			return slot;
    		}
    	}
    	return _container.appendBytes(transactionLogByteCount);
	}

	private void freeSlot(Slot slot){
    	if(slot == null){
    		return;
    	}
    	if(_container.freespaceManager() == null){
    	    return;
    	}
    	_container.freespaceManager().freeSafeSlot(slot);
	}

	public void applySlotChanges(Visitable<SlotChange> slotChangeTree, int slotChangeCount, Slot reservedSlot) {
		if(slotChangeCount > 0){
				
		    Slot transactionLogSlot = slotLongEnoughForLog(slotChangeCount, reservedSlot) ? reservedSlot
			    	: allocateSlot(true, slotChangeCount);
	
			    final StatefulBuffer buffer = new StatefulBuffer(_container.systemTransaction(), transactionLogSlot);
			    buffer.writeInt(transactionLogSlot.length());
			    buffer.writeInt(slotChangeCount);
	
			    appendSlotChanges(buffer, slotChangeTree);
	
			    buffer.write();
			    
			    Runnable commitHook = _container.commitHook();
			    
			    flushDatabaseFile();
	
			    _container.writeTransactionPointer(transactionLogSlot.address());
			    flushDatabaseFile();
	
			    if (writeSlots(slotChangeTree)) {
			    	flushDatabaseFile();
			    }
	
			    _container.writeTransactionPointer(0);
			    
			    
			    commitHook.run();
			    flushDatabaseFile();
			    
			    if (transactionLogSlot != reservedSlot) {
			    	freeSlot(transactionLogSlot);
			    }
		}
		freeSlot(reservedSlot);
	}
	
	private boolean slotLongEnoughForLog(int slotChangeCount, Slot slot){
    	return slot != null  &&  slot.length() >= transactionLogSlotLength(slotChangeCount);
    }
    

	public void close() {
		// do nothing
	}

}
