/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

/**
 * @exclude
 */
public interface ModificationAware {
	
	public boolean isModified(Object obj);

}
