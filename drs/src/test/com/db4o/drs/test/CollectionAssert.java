package com.db4o.drs.test;

import java.util.*;

import com.db4o.foundation.*;

import db4ounit.*;

public class CollectionAssert {

	public static void areEqual(Iterable expected, Iterable actual) {
		Iterator4Assert.areEqual(CollectionAssert.adapt(expected.iterator()), CollectionAssert.adapt(actual.iterator()));
	}

	static Iterator4 adapt(Iterator iterator) {
		return ReplicationTestPlatform.adapt(iterator);
	}

}
