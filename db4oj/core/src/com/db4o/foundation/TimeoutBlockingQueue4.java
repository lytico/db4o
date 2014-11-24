/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.foundation;

public interface TimeoutBlockingQueue4<T> extends PausableBlockingQueue4<T> {

	void check();

	void reset();
	
}
