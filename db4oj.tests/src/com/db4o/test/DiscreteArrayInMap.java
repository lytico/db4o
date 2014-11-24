/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.util.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class DiscreteArrayInMap {
	
	public Map map;
	
	public void storeOne(){
		map = new HashMap();
		map.put("atoms", new Atom[]{new Atom("1")});
	}
	
	public void testOne(){
	    Object arr = map.get("atoms");
	    Test.ensure(arr instanceof Atom[]);
	}
}
