/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;


public class QueryByInterfaceWithStored extends QueryByInterfaceBase {

	public void store() {
		Test.objectContainer().store(new Bar(0));
		Test.objectContainer().store(new Bar(1));
		Test.objectContainer().store(new Baz("A"));
	}
	
	public void XtestSODA() {
		assertSODA("A",1);
		assertSODA("B",0);
	}

	public void testEvaluation() {
		assertEvaluation("A",2);
		assertEvaluation("B",1);
	}
}
