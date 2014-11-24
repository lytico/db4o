/* Copyright (C) 2011   Versant Inc.   http://www.db4o.com */
package com.db4o.consistency;

import com.db4o.internal.slots.*;

public class IdObjectSlotDetail extends SlotDetail {

	private final int _id;

	public IdObjectSlotDetail(int id, Slot slot) {
		super(slot);
		_id = id;
	}
	
	public int id() {
		return _id;
	}
	
	@Override
	public String toString() {
		return "OBJ: " + _slot + "(" + _id + ")";
	}

}
