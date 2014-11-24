package com.db4o.db4ounit.jre12.staging;

import java.util.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class HashMapTestCase extends AbstractDb4oTestCase implements OptOutMultiSession {
	public static void main(String[] args) {
		new HashMapTestCase().runSolo();
	}

	protected void store() throws Exception {
		HashMap hashmap = new HashMap();
		for (int i = 0; i < 42; ++i) {
			hashmap.put(new Integer(i), "hello" + i);
		}
		store(hashmap);
	}

	public void test() throws Exception {
		HashMap hashmap = (HashMap) retrieveOnlyInstance(HashMap.class);
		for (int i = 0; i < 10; ++i) {
			store(hashmap);
			db().commit();
		}
		long oldSize = db().systemInfo().totalSize();
		store(hashmap);
		db().commit();
		long newSize = db().systemInfo().totalSize();
		Assert.areEqual(oldSize, newSize);
	}
}
