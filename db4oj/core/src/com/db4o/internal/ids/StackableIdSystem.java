/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.ids;

/**
 * @exclude
 */
public interface StackableIdSystem extends IdSystem {
	
	public int childId();
	
	public void childId(int id);

}
