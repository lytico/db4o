/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

public class GetAll {
	
	public void test(){
		int size = allObjectCount(); 
		Test.store(new GetAll());
		Test.ensure(allObjectCount() == size + 1);
		Test.rollBack();
		Test.ensure(allObjectCount() == size);
		Test.store(new GetAll());
		Test.ensure(allObjectCount() == size + 1);
		Test.commit();
		Test.ensure(allObjectCount() == size + 1);
	}
	
	private int allObjectCount(){
		return Test.objectContainer().queryByExample(null).size();
	}
	


}
