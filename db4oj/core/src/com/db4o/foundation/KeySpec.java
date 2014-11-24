/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public class KeySpec {
	
	public interface Deferred {
		Object evaluate();
	}
	
	private Object _defaultValue;
	
	public KeySpec(byte defaultValue) {
		_defaultValue = new Byte(defaultValue);
	}

	public KeySpec(int defaultValue) {
		_defaultValue = new Integer(defaultValue);
	}

	public KeySpec(boolean defaultValue) {
		_defaultValue = new Boolean(defaultValue);
	}

	public KeySpec(Object defaultValue) {
		_defaultValue = defaultValue;
	}

	public Object defaultValue() {
		if (_defaultValue instanceof Deferred) {
			return ((Deferred)_defaultValue).evaluate();
		}
		return _defaultValue;
	}		
}