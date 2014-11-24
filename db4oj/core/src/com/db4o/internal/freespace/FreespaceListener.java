/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.internal.freespace;

/**
 * @exclude
 */
public interface FreespaceListener {
	
	void slotAdded(int size);
	
	void slotRemoved(int size);

}
