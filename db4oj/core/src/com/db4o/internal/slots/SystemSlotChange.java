/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.slots;

import com.db4o.internal.ids.*;

/**
 * @exclude
 */
public class SystemSlotChange extends SlotChange {

	public SystemSlotChange(int id) {
		super(id);
	}
	
	@Override
	public void accumulateFreeSlot(TransactionalIdSystemImpl idSystem,
			FreespaceCommitter freespaceCommitter, boolean forFreespace) {
		super.accumulateFreeSlot(idSystem, freespaceCommitter, forFreespace);
		
		// FIXME: If we are doing a delete, we should also free our pointer here.
		
	}
	
	@Override
	protected Slot modifiedSlotInParentIdSystem(TransactionalIdSystemImpl idSystem) {
		return null;
	}
	
	@Override
	public boolean removeId() {
		return _newSlot == Slot.ZERO;
	}

}
