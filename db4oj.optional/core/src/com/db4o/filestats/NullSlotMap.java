/* Copyright (C) 2010   Versant Inc.   http://www.db4o.com */

package com.db4o.filestats;

import java.util.*;

import com.db4o.internal.slots.*;

/**
* @exclude
*/
@decaf.Ignore(decaf.Platform.JDK11)
public class NullSlotMap implements SlotMap {

	public void add(Slot slot) {
	}

	public List<Slot> merged() {
		return new ArrayList<Slot>();
	}

	public List<Slot> gaps(long length) {
		return new ArrayList<Slot>();
	}

	@Override
	public String toString() {
		return "";
	}
}
