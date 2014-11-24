package com.db4o.db4ounit.jre12.collections;

import java.util.*;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class HashMapActivationTestCase extends AbstractDb4oTestCase{
	
	public static class Holder{
		 public HashMap _hashmap;
	}
	
	protected void store() throws Exception {
		Holder holder = new Holder();
		holder._hashmap = new HashMap();
		holder._hashmap.put("key", "value");
		store(holder);
	}
	
	protected void configure(Configuration config) throws Exception {
		config.activationDepth(1);
	}
	
	public void test(){
		Holder holder = (Holder) retrieveOnlyInstance(Holder.class);
		db().activate(holder,2);
		db().activate(holder._hashmap, Integer.MAX_VALUE);
		Assert.areEqual(1, holder._hashmap.size());
	}

}
