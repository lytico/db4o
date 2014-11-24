/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;


public class QueryByInterfaceNoneStored extends QueryByInterfaceBase {	
	public void testSODA() {
		assertSODA("A",0);
	}

	public void testEvaluation() {
		assertEvaluation("A",0);
	}
}
