/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.util;

/**
 * @sharpen.ignore
 */
public class StopWatch {
	
	private long _started;

	private long _finished;
	
	public StopWatch() {
	}
	
	public void start() {
		_started = System.currentTimeMillis();
	}
	
	public void stop() {
		_finished = System.currentTimeMillis();
	}
	
	public long elapsed() {
		return _finished - _started;
	}
	
	public String toString() {
		long total = elapsed();
		if (total > 1000) {
			return total/1000.0 + "s";
		}
		return total + "ms";
	}
}
