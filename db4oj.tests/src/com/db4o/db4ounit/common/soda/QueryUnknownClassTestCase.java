/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.soda;

import java.util.*;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class QueryUnknownClassTestCase extends AbstractDb4oTestCase {

	public static class Data {
		public int _id;

		public Data(int id) {
			_id = id;
		}
	}

	public static class DataHolder {
		public Vector _data;

		public DataHolder(Object data) {
			_data = new Vector();
			_data.addElement(data);
		}
	}

	public static class Unrelated {
		public int _uid;

		public Unrelated(int id) {
			_uid = id;
		}
	}
	
	public void testQueryUnknownClass() {
		Query query = newQuery(Data.class);
		query.descend("_id").constrain(new Integer(42));
		ObjectSet result = query.execute();
		Assert.areEqual(0, result.size());
	}

	public void testQueryUnknownClassInUnknownCollection() {
		Query query = newQuery(DataHolder.class);
		query.descend("_data").descend("_id").constrain(new Integer(42));
		ObjectSet result = query.execute();
		Assert.areEqual(0, result.size());
	}

	public void _testQueryUnknownClassInCollection() {
		store(new DataHolder(new Unrelated(42)));
		Query query = newQuery(DataHolder.class);
		query.descend("_data").descend("_id").constrain(new Integer(42));
		ObjectSet result = query.execute();
		Assert.areEqual(0, result.size());
	}

	public void _testQueryUnknownClassInCollectionConjunction() {
		store(new DataHolder(new Unrelated(42)));
		Query query = newQuery(DataHolder.class);
		query.descend("_data").descend("_id").constrain(new Integer(42)).and(
				query.descend("_data").descend("_uid").constrain(new Integer(42)));
		ObjectSet result = query.execute();
		Assert.areEqual(0, result.size());
	}

	public void testQueryUnknownClassInCollectionDisjunction() {
		store(new DataHolder(new Unrelated(42)));
		Query query = newQuery(DataHolder.class);
		query.descend("_data").descend("_id").constrain(new Integer(42)).or(
				query.descend("_data").descend("_uid").constrain(new Integer(42)));
		ObjectSet result = query.execute();
		Assert.areEqual(1, result.size());
	}
}
