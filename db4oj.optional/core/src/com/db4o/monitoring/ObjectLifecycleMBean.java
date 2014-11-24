/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.monitoring;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public interface ObjectLifecycleMBean {
	
	double getObjectsStoredPerSec();
	
	double getObjectsDeletedPerSec();

	double getObjectsActivatedPerSec();
	
	double getObjectsDeactivatedPerSec();
	
}
