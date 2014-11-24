/* Copyright (C) 2011   Versant Inc.   http://www.db4o.com */
package com.db4o.consistency;

import com.db4o.internal.slots.*;

public class FreespaceSlotDetail extends SlotDetail {

	public FreespaceSlotDetail(Slot slot) {
		super(slot);
	}

	@Override
	public String toString() {
		return "FRE: " + _slot;
	}
}
