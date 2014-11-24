/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.monitoring;

import javax.management.*;

import com.db4o.*;
import com.db4o.monitoring.internal.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ObjectLifecycle extends MBeanRegistrationSupport implements ObjectLifecycleMBean{
	
	private final TimedReading _activated = TimedReading.newPerSecond();
	
	private final TimedReading _deactivated = TimedReading.newPerSecond();
	
	private final TimedReading _stored = TimedReading.newPerSecond();
	
	private final TimedReading _deleted = TimedReading.newPerSecond();

	public ObjectLifecycle(ObjectContainer db, Class<?> type) throws JMException {
		super(db, type);
	}

	public double getObjectsActivatedPerSec() {
		return _activated.read();
	}

	public double getObjectsDeactivatedPerSec() {
		return _deactivated.read();
	}

	public double getObjectsDeletedPerSec() {
		return _deleted.read();
	}

	public double getObjectsStoredPerSec() {
		return _stored.read();
	}

	public void notifyStored() {
		_stored.increment();
	}

	public void notifyDeleted() {
		_deleted.increment();
	}

	public void notifyActivated() {
		_activated.increment();
	}

	public void notifyDeactivated() {
		_deactivated.increment();
	}

}
