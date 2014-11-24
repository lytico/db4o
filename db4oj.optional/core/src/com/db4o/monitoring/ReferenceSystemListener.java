/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.monitoring;

/**
 * @exclude
 */
public interface ReferenceSystemListener {

	void notifyReferenceCountChanged(int changedBy);

}