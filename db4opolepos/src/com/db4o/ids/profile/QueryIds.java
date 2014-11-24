/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.ids.profile;

import static com.db4o.ids.profile.IdSysProfileUtil.*;

import com.db4o.foundation.*;
import com.db4o.internal.ids.*;

public class QueryIds {

	public static void main(String[] args) {
		final int firstId = 26;
		Procedure4<IdSystem> block = new Procedure4<IdSystem>() {
			public void apply(IdSystem idSystem) {
				for (int idIdx = 0; idIdx < NUM_IDS; idIdx++) {
					idSystem.committedSlot(firstId + idIdx);
				}
			}
		};
		long start = System.nanoTime();
		withIdSystem(block);
		long stop = System.nanoTime();
		long duration = (stop - start) / 1000000;
		System.out.println("Time to get " + NUM_IDS + " IDs: " + duration + "ms");
	}
	
}
