/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.monitoring.internal;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public interface Clock {

	long currentTimeMillis();

}
