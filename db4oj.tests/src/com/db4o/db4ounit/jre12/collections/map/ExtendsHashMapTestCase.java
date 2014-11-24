package com.db4o.db4ounit.jre12.collections.map;

import java.util.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class ExtendsHashMapTestCase extends AbstractDb4oTestCase {

	public static class ExtendsHashMap extends HashMap {
	}

	protected void store() throws Exception {
		Map map = new ExtendsHashMap();
		map.put(new Integer(1), "one");
		map.put(new Integer(2), "two");
		map.put(new Integer(3), "three");
		store(map);
	}
	
	public void test(){
		ExtendsHashMap ehm = (ExtendsHashMap)retrieveOnlyInstance(ExtendsHashMap.class);
		Assert.areEqual("one", ehm.get(new Integer(1)));
		Assert.areEqual("two", ehm.get(new Integer(2)));
		Assert.areEqual("three", ehm.get(new Integer(3)));
	}

}
