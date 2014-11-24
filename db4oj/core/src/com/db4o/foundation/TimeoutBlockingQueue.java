/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.foundation;

public class TimeoutBlockingQueue<T> extends PausableBlockingQueue<T> implements TimeoutBlockingQueue4<T> {

	private long expirationDate;
	private final long maxTimeToRemainPaused;

	public TimeoutBlockingQueue(long maxTimeToRemainPaused) {
		this.maxTimeToRemainPaused = maxTimeToRemainPaused;
	}

	@Override
	public boolean pause() {
		reset();
		return super.pause();
	}
	
	public void check() {
		long now = System.currentTimeMillis();
		if (now > expirationDate) {
			resume();
		}
	}

	public void reset() {
		expirationDate = System.currentTimeMillis() + maxTimeToRemainPaused;
	}

}
