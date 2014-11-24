/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.slots;

import com.db4o.foundation.*;
import com.db4o.internal.freespace.*;
import com.db4o.internal.ids.*;

/**
 * @exclude
 */
public class IdSystemSlotChange extends SystemSlotChange {
	
	private Collection4 _freed;

	public IdSystemSlotChange(int id) {
		super(id);
	}
	
	@Override
	protected void free(FreespaceManager freespaceManager, Slot slot) {
		if(slot.isNull()){
			return;
		}
		if(_freed == null){
			_freed = new Collection4();
		}
		_freed.add(slot);
	}
	
	@Override
	public void accumulateFreeSlot(TransactionalIdSystemImpl idSystem, FreespaceCommitter freespaceCommitter, boolean forFreespace) {
        if( forFreespace() != forFreespace){
        	return;
        }
		super.accumulateFreeSlot(idSystem, freespaceCommitter, forFreespace);
		if(_freed == null){
			return;
		}
		Iterator4 iterator = _freed.iterator();
		while(iterator.moveNext()){
			freespaceCommitter.delayedFree((Slot) iterator.current(), freeToSystemFreespaceSystem());
		}
	}
	
	protected boolean freeToSystemFreespaceSystem(){
		return true;
	}

}
