/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.drs.test.staging;

import java.util.*;

import com.db4o.drs.test.*;


// DRS-118
// NOTE: This test does not necessarily reproduce the symptom.
public class MapElementCustomHashCodeTestCase extends EqualsHashCodeOverriddenTestCaseBase {

	public static class Holder {
		Map _map = new HashMap();
		
		public Holder(Item itemA, Item itemB) {
			_map.put(itemA, itemA);
			_map.put(itemB, itemB);
		}
	}
	
	public void testReplicatesMap() {
		assertReplicates(new Holder(new Item("item"), new Item("item")));
	}
}
