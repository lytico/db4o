/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.internal.freespace;

/**
 * @exclude
 */
public class NullFreespaceListener implements FreespaceListener{
	
	public static final FreespaceListener INSTANCE = new NullFreespaceListener();
	
	private NullFreespaceListener(){
		
	}

	public void slotAdded(int size) {
		// do nothing;
	}

	public void slotRemoved(int size) {
		// do nothing
	}

}
