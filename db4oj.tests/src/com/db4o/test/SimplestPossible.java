/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

public class SimplestPossible {
    
	public String name;
    
    
    public SimplestPossible() {
    }
    
    public SimplestPossible(String name) {
        this.name = name;
    }
	
	public void storeOne(){
		name = "sp";
	}
	
	public void testOne(){
		Test.ensure(name.equals("sp"));
	}
}
