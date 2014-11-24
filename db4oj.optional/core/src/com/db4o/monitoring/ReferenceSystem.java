/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.monitoring;

import javax.management.*;

import com.db4o.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ReferenceSystem extends MBeanRegistrationSupport implements ReferenceSystemMBean, ReferenceSystemListener {
	
	private int _objectReferenceCount;

	public ReferenceSystem(ObjectContainer db, Class<?> type) throws JMException {
		super(db, type);
	}

	public int getObjectReferenceCount() {
		return _objectReferenceCount;
	}
	
	public void notifyReferenceCountChanged(int changedBy) {
		_objectReferenceCount += changedBy;
	}

}
