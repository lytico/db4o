/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.monitoring.internal;

import static com.db4o.foundation.Environments.my;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class TimedReading {
	
	private static final int IDENTITY_MS_FACTOR = 1;
	
	private final Clock _clock = my(Clock.class);
	
	private final int _msFactor;
	private int _count;
	private long _lastAccessTime = currentTimeMillis();

	public TimedReading() {
		this(IDENTITY_MS_FACTOR);
	}

	public TimedReading(int msFactor) {
		_msFactor = msFactor;
	}
	
	public void incrementBy(int count) {
		_count += count;
	}
	
	public void increment(){
		_count++;
	}
	
	public double read() {
		long curTime = currentTimeMillis();
		long timeDiff = curTime - _lastAccessTime;
		
		double reading = timeDiff > 0
			? ((double)_count * _msFactor / timeDiff)
			: 0;
			
		_lastAccessTime = curTime;
		_count = 0;
		return reading;
	}
	
	public int peek() {
		return _count;
	}
	
	private long currentTimeMillis() {
		return _clock.currentTimeMillis();
	}
	
	@Override
	public String toString() {
		return getClass().getSimpleName() + "(" + _count + ")";
	}

	public static TimedReading newPerSecond() {
		return new TimedReading(1000);
	}

	public void resetCount() {
		_count = 0;
		_lastAccessTime = currentTimeMillis();
	}

}
