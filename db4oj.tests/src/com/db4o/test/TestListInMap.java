/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.util.*;

import com.db4o.ext.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TestListInMap {

	public Map map;
	
	public void storeOne() {
	    ExtObjectContainer db = Test.objectContainer();
		List list = new LinkedList();
		list.add("ListEntry 1");
		db.store(list);
		map = new HashMap(); 			
		map.put("1", list);
	}
	
	public void testOne() {
	    List list = (List) map.get("1");
	    Object obj = list.get(0);
		System.out.println(obj);
	}
}

