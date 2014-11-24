/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public final class SimpleTimer implements Runnable {

	private final Runnable _runnable;

	private final long _interval;

	private Lock4 _lock;

	public volatile boolean stopped = false;

	public SimpleTimer(Runnable runnable, long interval) {
		_runnable = runnable;
		_interval = interval;
		_lock = new Lock4();
	}

	public void stop() {
		stopped = true;
		
		_lock.run(new Closure4() { 
			public Object run() {
				_lock.awake();
				return null;
			}
		});
	}

	public void run() {
		while (!stopped) {
			_lock.run(new Closure4() { 
				public Object run() {
					_lock.snooze(_interval);
					return null;
				}
			});
		
			if (!stopped) {
				_runnable.run();
			}
		}
	}
}
