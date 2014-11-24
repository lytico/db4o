/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.ids.profile;

import static com.db4o.ids.profile.IdSysProfileUtil.*;


public class CreateDB {

	public static void main(String[] args) {
		clearDatabase();
		long start = System.nanoTime();
		generateIDs(NUM_IDS);
		long stop = System.nanoTime();
		long duration = (stop - start) / 1000000;
		System.out.println("Time to generate database with " + NUM_IDS + " IDs: " + duration + "ms");
	}

	
}
