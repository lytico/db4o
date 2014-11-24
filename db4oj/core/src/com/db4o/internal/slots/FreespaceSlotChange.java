/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.slots;

/**
 * @exclude
 */
public class FreespaceSlotChange extends IdSystemSlotChange {

	public FreespaceSlotChange(int id) {
		super(id);
	}
	
	@Override
	protected boolean forFreespace() {
		return true;
	}

}
