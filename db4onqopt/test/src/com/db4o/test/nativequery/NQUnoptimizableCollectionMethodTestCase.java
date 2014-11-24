/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.test.nativequery;

import java.util.*;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.query.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class NQUnoptimizableCollectionMethodTestCase extends AbstractDb4oTestCase implements OptOutMultiSession {

	public static class Item {
		public ArrayList<String> _data;
		
		public Item(int size) {
			_data = new ArrayList<String>(size);
			for (int i = 0; i < size; i++) {
				_data.add(String.valueOf(i));
			}
		}
	}

	private static final int MAX_SIZE = 5;
	
	@Override
	protected void store() throws Exception {
		for (int i = 0; i < MAX_SIZE; i++) {
			store(new Item(i));
		}
	}

	public void testSize() {
		assertNotOptimized(new Predicate<Item>() {
			@Override
			public boolean match(Item candidate) {
				return candidate._data.size() == MAX_SIZE - 1;
			}
		}, 1);
	}

	public void testIsEmpty() {
		assertNotOptimized(new Predicate<Item>() {
			@Override
			public boolean match(Item candidate) {
				return candidate._data.isEmpty();
			}
		}, 1);
	}

	private void assertNotOptimized(Predicate<Item> predicate, int expectedSize) {
		final BooleanByRef optimized = new BooleanByRef();
		((LocalObjectContainer)db()).getNativeQueryHandler().addListener(new Db4oQueryExecutionListener() {
			public void notifyQueryExecuted(NQOptimizationInfo info) {
				optimized.value = info.optimized() != null;
			}
		});
		ObjectSet<Item> result = db().query(predicate);
		Assert.areEqual(expectedSize, result.size());
		Assert.isFalse(optimized.value);
	}
}
