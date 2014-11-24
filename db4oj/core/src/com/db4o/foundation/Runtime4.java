/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * A collection of static methods that should be part of the runtime environment but are not.
 * 
 * @exclude
 */
public class Runtime4 {

	/**
	 * sleeps without checked exceptions
	 */
	public static void sleep(long millis) {
		try {
			Thread.sleep(millis);
		} catch (Exception ignored) {
			
   		}
	}
	
	/**
	 * sleeps with implicit exception
	 */
	public static void sleepThrowsOnInterrupt(long millis) throws RuntimeInterruptedException {
		try {
			Thread.sleep(millis);
		} catch (InterruptedException e) {
			throw new RuntimeInterruptedException(e.toString());
   		}
	}
	
	/**
	 * Keeps executing a block of code until it either returns true or millisecondsTimeout
	 * elapses.
	 */
	public static boolean retry(long millisecondsTimeout, Closure4<Boolean> block) {
		return retry(millisecondsTimeout, 1, block);
	}
	
	
	public static boolean retry(long millisecondsTimeout, int millisecondsBetweenRetries, Closure4<Boolean> block) {
		final StopWatch watch = new AutoStopWatch();
		do {
			if (block.run()) {
				return true;
			}
			sleep(millisecondsBetweenRetries);
		} while (watch.peek() < millisecondsTimeout);
		return false;
	}


}
