/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.util.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class KeepCollectionContent {
	
	public void store(){
		Test.deleteAllInstances(new ComparableAtom());
		Test.deleteAllInstances(new HashMap());
		Test.deleteAllInstances(new Hashtable());
		Test.deleteAllInstances(new ArrayList());
		Test.deleteAllInstances(new Vector());
		Test.deleteAllInstances(new TreeMap());
		HashMap hm = new HashMap();
		hm.put(new ComparableAtom(), new ComparableAtom());
		Test.store(hm);
		Hashtable ht = new Hashtable();
		ht.put(new ComparableAtom(), new ComparableAtom());
		Test.store(ht);
		ArrayList al = new ArrayList();
		al.add(new ComparableAtom());
		Test.store(al);
		Vector vec = new Vector();
		vec.add(new ComparableAtom());
		Test.store(vec);
		TreeMap tm = new TreeMap();
		tm.put(new ComparableAtom(), new ComparableAtom());
		Test.store(tm);
		Test.commit();
	}
	
	public void test(){
		Test.deleteAllInstances(new HashMap());
		Test.deleteAllInstances(new Hashtable());
		Test.deleteAllInstances(new ArrayList());
		Test.deleteAllInstances(new Vector());
		Test.deleteAllInstances(new TreeMap());
		Test.commit();
		Test.ensureOccurrences(new ComparableAtom(), 8);
	}

}
