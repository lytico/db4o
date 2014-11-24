/* Copyright (C) 2004 - 2006  db4objects Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * @exclude
 */
public class TimeStampIdGenerator {
	private long _next;

	public static long idToMilliseconds(long id) {
		return id >> 15;
	}

	public TimeStampIdGenerator() {
		this(0);
	}

	public TimeStampIdGenerator(long minimumNext) {
		_next = minimumNext;
	}

	public long generate() {
		long t = System.currentTimeMillis();

		t = t << 15;

		if (t <= _next) {
			_next ++;
		} else {
			_next = t;
		}
		return _next;
	}

	public long minimumNext() {
		return _next;
	}

	public void setMinimumNext(long newMinimum) {
		_next = newMinimum;
	}
}
