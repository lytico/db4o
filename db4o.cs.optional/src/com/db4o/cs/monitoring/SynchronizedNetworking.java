package com.db4o.cs.monitoring;

import javax.management.*;

import com.db4o.*;

import decaf.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=Platform.JMX)
public class SynchronizedNetworking extends Networking {

	public SynchronizedNetworking(ObjectContainer db, Class<?> type) throws JMException {
		super(db, type);
	}

	@Override
	public synchronized double getBytesReceivedPerSecond() {
		return super.getBytesReceivedPerSecond();
	}
	
	@Override
	public synchronized double getBytesSentPerSecond() {
		return super.getBytesSentPerSecond();
	}
	
	@Override
	public synchronized double getMessagesSentPerSecond() {
		return super.getMessagesSentPerSecond();
	}

	@Override
	public synchronized void notifyRead(int count) {
		super.notifyRead(count);
	}
	
	@Override
	public synchronized void notifyWrite(int count) {
		super.notifyWrite(count);
	}
	
	@Override
	public synchronized void resetCounters() {
		super.resetCounters();
	}
}
