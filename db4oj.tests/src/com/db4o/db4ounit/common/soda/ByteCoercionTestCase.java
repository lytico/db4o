/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.soda;

import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ByteCoercionTestCase extends AbstractDb4oTestCase {
	
	public static class Item {
		public byte _b;
		
		public Item(byte b) {
			_b = b;
		}
	}

	@Override
	protected void store() throws Exception {
		store(new Item((byte)42));
	}
	
	public void testByteCoercion() {
		Query query = newQuery(Item.class);
		query.descend("_b").constrain(new Integer(42));
		Assert.areEqual(1, query.execute().size());
	}
}
