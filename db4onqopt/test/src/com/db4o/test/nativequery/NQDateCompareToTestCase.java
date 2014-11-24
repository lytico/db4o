/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.test.nativequery;

import java.util.*;

import com.db4o.*;
import com.db4o.internal.query.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class NQDateCompareToTestCase extends AbstractDb4oTestCase {

	public static class Item {
		public Date _date;

		public Item(Date date) {
			_date = date;
		}
	}

	@Override
	protected void store() throws Exception {
		store(new Item(new Date()));
	}
	
	public void testSingleDateCompareTo() {
		final Date cmpDate = new Date(0);
		Predicate<Item> predicate = new Predicate<Item>() {
			@Override
			public boolean match(Item item) {
				return item._date.compareTo(cmpDate) >= 0;
			}
		};
		assertDateComparison(predicate);
	}

	public void testMultipleDateCompareTo() {
		final Date cmpDatePre = new Date(0);
		final Date cmpDatePost = new Date(Long.MAX_VALUE);
		Predicate<Item> predicate = new Predicate<Item>() {
			@Override
			public boolean match(Item item) {
				return item._date.compareTo(cmpDatePre) >= 0 && item._date.compareTo(cmpDatePost) < 0;
			}
		};
		assertDateComparison(predicate);
	}

	private void assertDateComparison(Predicate<Item> predicate) {
		container().getNativeQueryHandler().addListener(new Db4oQueryExecutionListener() {
			@Override
			public void notifyQueryExecuted(NQOptimizationInfo info) {
				Assert.isNotNull(info.optimized());
			}
		});
		ObjectSet<Item> result = db().query(predicate);
		Assert.areEqual(1, result.size());
	}

}
