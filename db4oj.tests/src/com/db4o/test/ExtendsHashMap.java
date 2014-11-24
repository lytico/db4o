/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.util.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class ExtendsHashMap extends HashMap{
	
	public void store(){
		Test.deleteAllInstances(this);
		put(new Integer(1), "one");
		put(new Integer(2), "two");
		put(new Integer(3), "three");
		Test.store(this);
	}
	
	public void test(){
		ExtendsHashMap ehm = (ExtendsHashMap)Test.getOne(this);
		Test.ensure(ehm.get(new Integer(1)).equals("one"));
		Test.ensure(ehm.get(new Integer(3)).equals("three"));
	}

}
