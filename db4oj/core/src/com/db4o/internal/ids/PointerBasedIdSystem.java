/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.ids;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.slots.*;
import com.db4o.internal.transactionlog.*;

/**
 * @exclude
 */
public final class PointerBasedIdSystem implements IdSystem {
	
	final TransactionLogHandler _transactionLogHandler;
	
	private final LocalObjectContainer _container;

	public PointerBasedIdSystem(LocalObjectContainer container) {
		_container = container;
		_transactionLogHandler = newTransactionLogHandler(container);
	}

	public int newId() {
		return _container.allocatePointerSlot();
	}

	public final Slot committedSlot(int id) {
		return _container.readPointerSlot(id);
	}

	public void commit(Visitable<SlotChange> slotChanges, FreespaceCommitter freespaceCommitter) {
		Slot reservedSlot = _transactionLogHandler.allocateSlot(false, countSlotChanges(slotChanges));
		freespaceCommitter.commit();
		_transactionLogHandler.applySlotChanges(slotChanges, countSlotChanges(slotChanges), reservedSlot);
	}

	private int countSlotChanges(Visitable<SlotChange> slotChanges) {
		final IntByRef slotChangeCount = new IntByRef();
		slotChanges.accept(new Visitor4<SlotChange>() {
			public void visit(SlotChange slotChange) {
                if(slotChange.slotModified()){
                	slotChangeCount.value++;
                }
			}
		});
		return slotChangeCount.value;
	}

	public void returnUnusedIds(Visitable<Integer> visitable) {
		visitable.accept(new Visitor4<Integer>() {
			public void visit(Integer id) {
				_container.free(id, Const4.POINTER_LENGTH);
			}
		});
	}
	
	private TransactionLogHandler newTransactionLogHandler(LocalObjectContainer container) {
		boolean fileBased = container.config().fileBasedTransactionLog() && container instanceof IoAdaptedObjectContainer;
		if(! fileBased){
			return new EmbeddedTransactionLogHandler(container);
		}
		String fileName = ((IoAdaptedObjectContainer)container).fileName();
		return new FileBasedTransactionLogHandler(container, fileName); 
	}

	public void close() {
		_transactionLogHandler.close();
	}

	public void completeInterruptedTransaction(
			int transactionId1, int transactionId2) {
		_transactionLogHandler.completeInterruptedTransaction(transactionId1, transactionId2);
	}

	@Override
	public void traverseOwnSlots(Procedure4<Pair<Integer, Slot>> block) {
	}

}
