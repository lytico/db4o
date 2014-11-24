/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.instrumentation.api;


public final class CallingConvention {
	
	public static final CallingConvention STATIC = new CallingConvention();
	
	public static final CallingConvention VIRTUAL = new CallingConvention();
	
	public static final CallingConvention INTERFACE = new CallingConvention();
	
	private CallingConvention() {
	}
}
