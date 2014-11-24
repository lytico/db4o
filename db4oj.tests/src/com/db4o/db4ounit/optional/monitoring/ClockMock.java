/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.optional.monitoring;

import com.db4o.monitoring.internal.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
final public class ClockMock implements Clock {
	
	private long _currentTime;

	public long currentTimeMillis() {
		return _currentTime;
	}

	public void advance(int time) {
		_currentTime += time;
	}
}
