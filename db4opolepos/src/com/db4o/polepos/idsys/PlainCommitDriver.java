/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.idsys;

import com.db4o.foundation.*;
import com.db4o.internal.ids.*;
import com.db4o.internal.slots.*;


public class PlainCommitDriver extends IdSystemDriver {

	private int[] ids;

	public PlainCommitDriver(IdSystemEngine engine) {
		super(engine);
	}
		
	public void lapAllocate() {
		ids = new int[setup().getObjectCount()];
		for (int idIdx = 0; idIdx < ids.length; idIdx++) {
			ids[idIdx] = idSystem().newId();
		}
	}

	public void fullCommit() {
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
	
	public void multipleCommits() {
		int commitCount = setup().getCommitCount();
		final int updateCount = setup().getUpdateCount();
		
		int idx = 0;
		for (int i = 0; i < commitCount; i++) {
			if(idx + updateCount > ids.length - 1){
				idx = 0;
			}
			final int finalIdx = idx;
			idSystem().commit(new Visitable<SlotChange>() {
				public void accept(Visitor4<SlotChange> visitor) {
					for (int idIdx = finalIdx; idIdx < finalIdx + updateCount; idIdx++) {
						SlotChange slotChange = new SlotChange(ids[idIdx]);
						slotChange.notifySlotCreated(new Slot(idIdx, 1));
						visitor.visit(slotChange);
					}
				}
			}, FreespaceCommitter.DO_NOTHING);
		}
		
	}
	
	public void fragmentedCommits() {
		final int commitCount = setup().getCommitCount();
		final int updateCount = setup().getUpdateCount();
		final IntByRef idx = new IntByRef();
		for (int commit = 0; commit < commitCount; commit++) {
			idSystem().commit(new Visitable<SlotChange>() {
				public void accept(Visitor4<SlotChange> visitor) {
					for (int i = 0; i < updateCount; i++) {
						idx.value += 37;
						if(idx.value >= ids.length - 1){
							idx.value = i;
						}
						SlotChange slotChange = new SlotChange(ids[idx.value]);
						slotChange.notifySlotCreated(new Slot(idx.value, 1));
						visitor.visit(slotChange);
					}
				}
			}, FreespaceCommitter.DO_NOTHING);
		}
	}
	

}
