/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;


/**
 * Useful as "out" or "by ref" function parameter.
 */
public final class LongByRef {
	
	public long value;
	
	public LongByRef(long initialValue) {
		value = initialValue;
	}
	
	public LongByRef() {
	}
}
