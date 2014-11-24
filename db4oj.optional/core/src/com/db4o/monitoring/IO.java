/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.monitoring;

import javax.management.*;

import com.db4o.*;
import com.db4o.monitoring.internal.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
class IO extends MBeanRegistrationSupport implements IOMBean {

	private TimedReading _numBytesReadPerSec = TimedReading.newPerSecond();
	
	private TimedReading _numBytesWrittenPerSec = TimedReading.newPerSecond();
	
	private TimedReading _numReadsPerSec = TimedReading.newPerSecond();
	
	private TimedReading _numWritesPerSec = TimedReading.newPerSecond();
	
	private TimedReading _numSyncsPerSec = TimedReading.newPerSecond();
	
	public IO(ObjectContainer db, Class<?> type) throws JMException {
		super(db, type);
	}

	public double getBytesReadPerSecond() {
		return _numBytesReadPerSec.read();
	}

	public double getBytesWrittenPerSecond() {
		return _numBytesWrittenPerSec.read();
	}

	public double getReadsPerSecond() {
		return _numReadsPerSec.read();
	}

	public double getWritesPerSecond() {
		return _numWritesPerSec.read();
	}

	public double getSyncsPerSecond() {
		return _numSyncsPerSec.read();
	}

	public void notifyBytesRead(int numBytesRead) {
		_numBytesReadPerSec.incrementBy(numBytesRead);
		_numReadsPerSec.increment();
	}

	public void notifyBytesWritten(int numBytesWritten) {
		_numBytesWrittenPerSec.incrementBy(numBytesWritten);
		_numWritesPerSec.increment();
	}

	public void notifySync() {
		_numSyncsPerSec.increment();
	}

}
