/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.ta.instrumentation;

/**
 * @exclude
 */
public abstract class TransparentActivationInstrumentationConstants {

	public final static String ACTIVATOR_FIELD_NAME = "_db4o$$ta$$activator";
	public final static String BIND_METHOD_NAME = "bind";
	public static final String INIT_METHOD_NAME = "<init>";
	public static final String ACTIVATE_METHOD_NAME = "activate";
	public static final String ACTIVATOR_ACTIVATE_METHOD_NAME = ACTIVATE_METHOD_NAME;
	
	private TransparentActivationInstrumentationConstants() {}
}
