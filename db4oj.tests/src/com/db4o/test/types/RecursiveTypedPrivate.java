/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class RecursiveTypedPrivate extends RTest
{
	private RecursiveTypedPrivate recurse;
	private String depth;
	
	public void set(int ver){
		set(ver, 10);
	}
	
	public void set(int ver, int a_depth){
		depth = "s" + ver + ":" +  a_depth;
		if(a_depth > 0){
			recurse = new RecursiveTypedPrivate();
			recurse.set(ver, a_depth - 1);
		}
	}
	
	public boolean jdk2(){
		return true;
	}

}
