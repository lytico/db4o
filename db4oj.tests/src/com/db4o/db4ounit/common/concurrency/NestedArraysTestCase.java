/* Copyright (C) 2004 - 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class NestedArraysTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new NestedArraysTestCase().runConcurrency();
	}

	public Object _object;

	public Object[] _objectArray;

	private static final int DEPTH = 5;

	private static final int ELEMENTS = 3;

	public NestedArraysTestCase() {

	}

	protected void store() {
		_object = new Object[ELEMENTS];
		fill((Object[]) _object, DEPTH);

		_objectArray = new Object[ELEMENTS];
		fill(_objectArray, DEPTH);
		store(this);
	}

	private void fill(Object[] arr, int depth) {

		if (depth <= 0) {
			arr[0] = "somestring";
			arr[1] = new Integer(10);
			return;
		}

		depth--;

		for (int i = 0; i < ELEMENTS; i++) {
			arr[i] = new Object[ELEMENTS];
			fill((Object[]) arr[i], depth);
		}
	}

	public void conc(ExtObjectContainer oc) {
		NestedArraysTestCase nr = (NestedArraysTestCase) retrieveOnlyInstance(oc, NestedArraysTestCase.class);
		check((Object[]) nr._object, DEPTH);
		check((Object[]) nr._objectArray, DEPTH);
	}

	private void check(Object[] arr, int depth) {
		if (depth <= 0) {
			Assert.areEqual("somestring", arr[0]);
			Assert.areEqual(new Integer(10), arr[1]);
			return;
		}

		depth--;

		for (int i = 0; i < ELEMENTS; i++) {
			check((Object[]) arr[i], depth);
		}

	}

}
