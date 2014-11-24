/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.ids.profile;

import static com.db4o.ids.profile.IdSysProfileUtil.*;

import com.db4o.foundation.*;
import com.db4o.internal.ids.*;
import com.db4o.internal.slots.*;

public class CommitIds {
	
	private static int IDS_COUNT = 100000;
	
	private static int COMMIT_COUNT = 5000;

	public static void main(String[] args) {
		clearDatabase();
		generateIDs(IDS_COUNT);
		commitIDs(COMMIT_COUNT);
	}

	private static void commitIDs(final int commitCount) {
		Procedure4<IdSystem> block = new Procedure4<IdSystem>() {
			public void apply(IdSystem idSystem) {
				final int[] ids = new int[commitCount];
				for (int idIdx = 0; idIdx < commitCount; idIdx++) {
					ids[idIdx] = idSystem.newId();
				}
				for (int idIdx = 0; idIdx < commitCount; idIdx++) {
					final int finalidIdx = idIdx;
					idSystem.commit(new Visitable<SlotChange>() {
						public void accept(Visitor4<SlotChange> visitor) {
							SlotChange change = new SlotChange(ids[finalidIdx]);
							change.notifySlotCreated(new Slot(finalidIdx, 1));
							visitor.visit(change);
						}
					}, FreespaceCommitter.DO_NOTHING);
				}
			}
		};
		long start = System.nanoTime();
		withIdSystem(block);
		long stop = System.nanoTime();
		long duration = (stop - start) / 1000000;
		System.out.println("Time to create and commit " + commitCount + " IDs");
		System.out.println("on a database with " + IDS_COUNT + " IDs: " + duration + "ms");
	}
	
}
