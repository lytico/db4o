/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.test.nativequery;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class NQGreaterOrEqualTestCase extends AbstractDb4oTestCase{
	
	public static class Item{
		
		public Item(long lowerLong, long upperLong) {
			_lowerLong = lowerLong;
			_upperLong = upperLong;
		}

		public long _lowerLong;
		
		public long _upperLong;
		
	}
	
	@Override
	protected void store() throws Exception {
		store(new Item(1, 100));
		store(new Item(50,300));
	}
	
	public void testNativeQueryForLong(){
		final long searchedLong = 20;
		ObjectSet<Item> objectSet = searchLong(searchedLong);
		Assert.areEqual(1, objectSet.size());
	}

	private ObjectSet<Item> searchLong(final long searchedLong) {
		ObjectSet<Item> objectSet = db().query(new Predicate<Item>() {
			@Override
			public boolean match(Item candidate) {
				return searchedLong>=candidate._lowerLong && searchedLong<=candidate._upperLong;
			}
		});
		return objectSet;
	}

}
