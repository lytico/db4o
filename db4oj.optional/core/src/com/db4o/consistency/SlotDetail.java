/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.consistency;

import com.db4o.internal.slots.*;

public abstract class SlotDetail {
	public final Slot _slot;

	public SlotDetail(Slot slot) {
		this._slot = slot;
	}
	
}