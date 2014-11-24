/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;

public class DualDelete {
	
	public Atom atom;
	
	public void configure(){
		Db4o.configure().objectClass(this).cascadeOnDelete(true);
		Db4o.configure().objectClass(this).cascadeOnUpdate(true);
	}
	
	public void store(){
		Test.deleteAllInstances(this);
		Test.deleteAllInstances(new Atom());
		DualDelete dd1 = new DualDelete();
		dd1.atom = new Atom("justone");
		Test.store(dd1);
		DualDelete dd2 = new DualDelete();
		dd2.atom = dd1.atom;
		Test.store(dd2);
	}
	
	public void test(){
		Test.deleteAllInstances(this);
		Test.ensureOccurrences(new Atom(), 0);
		Test.rollBack();
		Test.ensureOccurrences(new Atom(), 1);
		Test.deleteAllInstances(this);
		Test.ensureOccurrences(new Atom(), 0);
		Test.commit();
		Test.ensureOccurrences(new Atom(), 0);
		Test.rollBack();
		Test.ensureOccurrences(new Atom(), 0);
	}
	

}
