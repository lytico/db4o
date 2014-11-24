/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import java.util.*;

import com.db4o.*;

public class TestHashTable {
	
	public Hashtable ht;
	
	public void configure(){
		Db4o.configure().objectClass(this).cascadeOnUpdate(true);
		Db4o.configure().objectClass(this).cascadeOnDelete(true);
	}
	
	public void store(){
		Test.deleteAllInstances(this);
		Test.deleteAllInstances(new Atom());
		Test.deleteAllInstances(new com.db4o.config.Entry());
		TestHashTable tht = new TestHashTable();
		tht.ht = new Hashtable();
		tht.ht.put("t1", new Atom("t1"));
		tht.ht.put("t2", new Atom("t2"));
		Test.store(tht);
	}
	
	public void test(){
		com.db4o.config.Entry checkEntries = new com.db4o.config.Entry();
		TestHashTable tht = (TestHashTable)Test.getOne(this);
		Test.ensure(tht.ht.size() == 2);
		Test.ensure(tht.ht.get("t1").equals(new Atom("t1")));
		Test.ensure(tht.ht.get("t2").equals(new Atom("t2")));
		tht.ht.put("t2", new Atom("t3"));
		Test.store(tht);
		
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
