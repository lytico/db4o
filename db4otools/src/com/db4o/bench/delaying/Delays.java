/* Copyright (C) 2004 - 2007 Versant Inc. http://www.db4o.com */

package com.db4o.bench.delaying;


public class Delays {

	public static final int READ = 0;
	public static final int WRITE = 1;
	public static final int SYNC = 3;
	
	public static final int COUNT = 4;

	public static final String units = "nanoseconds";

	public long[] values;

	
	public Delays(long read, long write, long sync) {
		values = new long[] {read, write, sync};
	}
	
	public String toString() {
		return "[delays in " + units + "] read: " + values[READ] + " | write: " + values[WRITE] +
			 " | sync: " + values[SYNC];
	}
	
}
