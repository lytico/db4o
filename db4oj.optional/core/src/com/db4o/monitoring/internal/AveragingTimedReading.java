/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.monitoring.internal;

import static com.db4o.foundation.Environments.my;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class AveragingTimedReading {
	
	private final Clock _clock = my(Clock.class);
	
	private long _lastStart;
	private long _aggregateTime;
	private int _eventCount;
	
	public synchronized void eventStarted() {
		_lastStart = currentTime();
	}

	public synchronized void eventFinished() {
		if (-1 == _lastStart) {
			throw new IllegalStateException();
		}
		
		_aggregateTime += currentTime() - _lastStart;
		_eventCount++;
		_lastStart = -1;
	}
	
	public synchronized double read() {
		if (_eventCount == 0) {
			return 0;
		}
		final long value = _aggregateTime / _eventCount;
		_eventCount = 0;
		_aggregateTime = 0;
		_lastStart = currentTime();
		return value;
	}
	
	private long currentTime() {
		return _clock.currentTimeMillis();
	}

}
