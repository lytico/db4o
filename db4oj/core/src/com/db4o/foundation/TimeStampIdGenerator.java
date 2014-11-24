/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * @exclude
 */
public class TimeStampIdGenerator {
	
	public static final int BITS_RESERVED_FOR_COUNTER = 15;
	
	public static final int COUNTER_LIMIT = 64;
    
	private long _counter;
	
	private long _lastTime;

	public static long idToMilliseconds(long id) {
		return id >> BITS_RESERVED_FOR_COUNTER;
	}

	public static long millisecondsToId(long milliseconds) {
		return milliseconds << BITS_RESERVED_FOR_COUNTER;
	}
	
	public TimeStampIdGenerator(long minimumNext) {
		internalSetMinimumNext(minimumNext);
	}

	public TimeStampIdGenerator() {
		this(0);
	}

	public long generate() {
		long t = now();
		if(t > _lastTime){
			_lastTime = t;
			_counter = 0;
			return millisecondsToId(t);
		}
		updateTimeOnCounterLimitOverflow();
		_counter++;
		updateTimeOnCounterLimitOverflow();
		return last();
	}

	protected long now() {
		return System.currentTimeMillis();
	}

	private final void updateTimeOnCounterLimitOverflow() {
		if(_counter < COUNTER_LIMIT){
			return;
		}
		long timeIncrement = _counter / COUNTER_LIMIT;
		_lastTime += timeIncrement;
		_counter -= (timeIncrement * COUNTER_LIMIT);
	}

	public long last() {
		return millisecondsToId(_lastTime) + _counter;
	}

	public boolean setMinimumNext(long newMinimum) {
        if(newMinimum <= last()){
            return false;
        }
        internalSetMinimumNext(newMinimum);
        return true;
	}

	private void internalSetMinimumNext(long newNext) {
		_lastTime = idToMilliseconds(newNext);
		long timePart = millisecondsToId(_lastTime);
		_counter = newNext - timePart;
		updateTimeOnCounterLimitOverflow();
	}
	
}
