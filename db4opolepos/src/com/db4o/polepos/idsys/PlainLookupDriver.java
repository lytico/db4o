/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.idsys;

import com.db4o.foundation.*;
import com.db4o.internal.ids.*;
import com.db4o.internal.slots.*;


public class PlainLookupDriver extends IdSystemDriver {

	private int[] ids;

	public PlainLookupDriver(IdSystemEngine engine) {
		super(engine);
	}
		
	public void lapAllocate() {
		ids = new int[setup().getObjectCount()];
		for (int idIdx = 0; idIdx < ids.length; idIdx++) {
			ids[idIdx] = idSystem().newId();
		}
		idSystem().commit(new Visitable<SlotChange>() {
			public void accept(Visitor4<SlotChange> visitor) {
				for (int idIdx = 0; idIdx < ids.length; idIdx++) {
					SlotChange slotChange = new SlotChange(ids[idIdx]);
					slotChange.notifySlotCreated(new Slot(idIdx, 1));
					visitor.visit(slotChange);
				}
			}
		}, FreespaceCommitter.DO_NOTHING);
	}

	public void fullLookup() {
		for (int idIdx = 0; idIdx < ids.length; idIdx++) {
			idSystem().committedSlot(ids[idIdx]);
		}
	}
	
	public void multipleLookups() {
		int selectCount = setup().getSelectCount();
		int size = setup().getObjectSize();
		int idx = 0;
		for (int i = 0; i < selectCount; i++) {
			if(idx + size > ids.length - 1){
				idx = 0;
			}
			for (int idIdx = idx; idIdx < idx + size; idIdx++) {
				idSystem().committedSlot(ids[idIdx]);		
			}
		}
	}
	
	public void fragmentedLookups() {
		int selectCount = setup().getSelectCount();
		int size = setup().getObjectSize();
		int idx = 0;
		for (int select = 0; select < selectCount; select++) {
			for (int i = 0; i < size; i++) {
				idx += 37;
				if(idx >= ids.length - 1){
					idx = i;
				}
				idSystem().committedSlot(ids[idx]);		
			}
		}
	}



}
