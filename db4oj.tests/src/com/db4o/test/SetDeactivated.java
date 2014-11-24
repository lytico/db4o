/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

public class SetDeactivated {
	
	public String foo;
	
	public SetDeactivated(){
	}
	
	public SetDeactivated(String foo){
		this.foo = foo;
	}
	
	public void store(){
		Test.deleteAllInstances(this);
		Test.store(new SetDeactivated("hi"));
		Test.commit();
	}
	
	public void test(){
		SetDeactivated sd = (SetDeactivated)Test.getOne(this);
		Test.objectContainer().deactivate(sd, 1);
		Test.store(sd);
		Test.objectContainer().purge(sd);
		sd = (SetDeactivated)Test.getOne(this);
		Test.objectContainer().activate(sd, 1);
		Test.ensure(sd.foo.equals("hi"));
	}
}
