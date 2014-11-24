/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;

public class HashSet4TestCase implements TestLifeCycle {

	private Set4 _set;
	
	public void testEmpty() {
		assertEmpty();
	}

	public void testSingleAdd() {
		Object obj = new Object();
		_set.add(obj);
		Assert.isFalse(_set.isEmpty());
		Assert.areEqual(1, _set.size());
		Assert.isTrue(_set.contains(obj));
		Assert.isFalse(_set.contains(new Object()));
		Iterator4 iter = _set.iterator();
		Assert.isTrue(iter.moveNext());
		Assert.areEqual(obj, iter.current());
	}

	public void testSingleRemove() {
		Object obj = new Object();
		_set.add(obj);
		Assert.isTrue(_set.remove(obj));
		assertEmpty();
	}

	public void testMultipleAddRemove() {
		Object[] objs = {
				new Object(),
				new Object(),
				new Object()
		};
		for (Object obj : objs) {
			_set.add(obj);
		}
		Assert.isFalse(_set.isEmpty());
		Assert.areEqual(objs.length, _set.size());
		for (Object obj : objs) {
			Assert.isTrue(_set.contains(obj));
		}
		Assert.isFalse(_set.contains(new Object()));
		Iterator4Assert.sameContent(objs, _set.iterator());
	}

	public void testClear() {
		Object[] objs = {
				new Object(),
				new Object(),
				new Object()
		};
		for (Object obj : objs) {
			_set.add(obj);
		}
		_set.clear();
		assertEmpty();
	}

	private void assertEmpty() {
		Assert.isTrue(_set.isEmpty());
		Assert.areEqual(0, _set.size());
		Assert.isFalse(_set.contains(new Object()));
		Assert.isFalse(_set.remove(new Object()));
		Assert.isFalse(_set.iterator().moveNext());
	}
	
	public void setUp() throws Exception {
		_set = new HashSet4();
	}

	public void tearDown() throws Exception {
	}
	
}
