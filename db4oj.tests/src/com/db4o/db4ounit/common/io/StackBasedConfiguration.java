/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */
package com.db4o.db4ounit.common.io;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class StackBasedConfiguration {
	public final String _className;
	public final String _methodName;
	public final int _hitThreshold;

	public StackBasedConfiguration(String className, String methodName, int hitThreshold) {
		_className = className;
		_methodName = methodName;
		_hitThreshold = hitThreshold;
	}
}
