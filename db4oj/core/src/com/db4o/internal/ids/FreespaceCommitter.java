/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.ids;

import java.util.*;

import com.db4o.foundation.*;
import com.db4o.internal.freespace.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public class FreespaceCommitter {
	
	public static final FreespaceCommitter DO_NOTHING = new NullFreespaceCommitter();
	
	private final List<Slot> _freeToUserFreespaceSystem = new ArrayList<Slot>();
	
	private final List<Slot> _freeToSystemFreespaceSystem = new ArrayList<Slot>();

	private final FreespaceManager _freespaceManager;
	
	private TransactionalIdSystem _transactionalIdSystem;
	
	public FreespaceCommitter(FreespaceManager freespaceManager) {
		_freespaceManager = freespaceManager == null ? NullFreespaceManager.INSTANCE : freespaceManager;
	}
	
	public void commit() {
		apply(_freeToUserFreespaceSystem);
		_freespaceManager.beginCommit();
		
		_freespaceManager.commit();
		
		_transactionalIdSystem.accumulateFreeSlots(this, true);
		
		apply(_freeToSystemFreespaceSystem);
		_freespaceManager.endCommit();
	}

	private void apply(List<Slot> toFree) {
		for(Slot slot : toFree){
			_freespaceManager.free(slot);
		}
		toFree.clear();
	}

	public void transactionalIdSystem(TransactionalIdSystem transactionalIdSystem) {
		_transactionalIdSystem = transactionalIdSystem;
	}
	
	private static class NullFreespaceCommitter extends FreespaceCommitter {

		public NullFreespaceCommitter() {
			super(NullFreespaceManager.INSTANCE);
		}
		
		@Override
		public void commit() {
			// do nothing
		}
		
	}

	public void delayedFree(Slot slot, boolean freeToSystemFreeSpaceSystem) {
		if(freeToSystemFreeSpaceSystem){
			_freeToSystemFreespaceSystem.add(slot);
		}else {
			_freeToUserFreespaceSystem.add(slot);
		}
	}

}
