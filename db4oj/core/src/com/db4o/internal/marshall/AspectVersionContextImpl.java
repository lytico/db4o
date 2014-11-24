/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

/**
 * @exclude
 */
public class AspectVersionContextImpl implements AspectVersionContext{
	
	private final int _declaredAspectCount;

	private AspectVersionContextImpl(int count) {
		_declaredAspectCount = count;
	}

	public int declaredAspectCount() {
		return _declaredAspectCount;
	}

	public void declaredAspectCount(int count) {
		throw new IllegalStateException();
	}
	
	public static final AspectVersionContextImpl ALWAYS_ENABLED = new AspectVersionContextImpl(Integer.MAX_VALUE);
	
	public static final AspectVersionContextImpl CHECK_ALWAYS_ENABLED = new AspectVersionContextImpl(Integer.MAX_VALUE - 1);

	public static AspectVersionContext forSize(int count) {
		return new AspectVersionContextImpl(count);
	}
	

}
