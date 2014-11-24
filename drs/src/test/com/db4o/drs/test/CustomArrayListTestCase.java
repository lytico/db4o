package com.db4o.drs.test;

import java.util.*;

import com.db4o.drs.test.data.*;

import db4ounit.*;

public class CustomArrayListTestCase extends DrsTestCase {
	
	public void test() {
		
		NamedList original = new NamedList("foo");
		original.add("bar");
		
		a().provider().storeNew(original);
		a().provider().commit();
		
		replicateAll(a().provider(), b().provider());
		
		Iterator iterator = b().provider().getStoredObjects(NamedList.class).iterator();
		Assert.isTrue(iterator.hasNext());
		NamedList replicated = (NamedList)iterator.next();
		Assert.areEqual(original.name(), replicated.name());
		CollectionAssert.areEqual(original, replicated);
	}

}
