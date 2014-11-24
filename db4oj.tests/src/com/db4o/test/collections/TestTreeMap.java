/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.collections;

import java.util.*;

import com.db4o.*;
import com.db4o.test.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TestTreeMap {
	
	TreeMap tm;
	
	public void configure(){
		Db4o.configure().objectClass(this).cascadeOnUpdate(true);
		Db4o.configure().objectClass(this).cascadeOnDelete(true);
	}
	
	public void store(){
		Test.deleteAllInstances(this);
		Test.deleteAllInstances(new Atom());
		Test.deleteAllInstances(new com.db4o.config.Entry());
		TestTreeMap ttm = new TestTreeMap();
		ttm.tm = new TreeMap();
		ttm.tm.put("t1", new Atom("t1"));
		ttm.tm.put("t2", new Atom("t2"));
		Test.store(ttm);
	}
	
	public void test(){
		com.db4o.config.Entry checkEntries = new com.db4o.config.Entry();
		TestTreeMap ttm = (TestTreeMap)Test.getOne(this);
		Test.ensure(ttm.tm.size() == 2);
		Test.ensure(ttm.tm.get("t1").equals(new Atom("t1")));
		Test.ensure(ttm.tm.get("t2").equals(new Atom("t2")));
		ttm.tm.put("t2", new Atom("t3"));
		Test.store(ttm);
		if(Test.COMPARE_INTERNAL_OK){
			Test.ensureOccurrences(checkEntries, 2);
			Test.commit();
			Test.ensureOccurrences(checkEntries, 2);
			Test.deleteAllInstances(this);
			Test.ensureOccurrences(checkEntries, 0);
			Test.rollBack();
			Test.ensureOccurrences(checkEntries, 2);
			Test.deleteAllInstances(this);
			Test.ensureOccurrences(checkEntries, 0);
			Test.commit();
			Test.ensureOccurrences(checkEntries, 0);
		}
	}
}
