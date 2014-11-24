/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation.samples.pathological;

/**
 * @exclude
 */
public class SuperDate {
	private long _ms;
	
	public boolean after(SuperDate date) {
		return _ms > date._ms;
	}
}
