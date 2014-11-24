/* Copyright (C) 2004 - 2007 Versant Inc. http://www.db4o.com */

package com.db4o.bench.timing;


public class NanoStopWatch {

		
	private long _started;
	private long _elapsed;
	
	private NanoTiming _timing;

	public NanoStopWatch() {
		_timing = new NanoTiming();
	}
	
	public void start() {
		_started = _timing.nanoTime();
	}
	
	public void stop() {
		_elapsed = _timing.nanoTime() - _started;
	}
	
	public long elapsed() {
//		System.out.println("NanoStopWatch.elapsed: " + _elapsed);
		return _elapsed;
	}
	
}
