/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * Useful as "out" or "by ref" function parameter.
 */
public class BooleanByRef {
	
	public boolean value;

	public BooleanByRef() {
		this(false);
	}

	public BooleanByRef(boolean value_) {
		value = value_;
	}
	
}
