/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class BiParentUnTypedPrivate extends RTest
{
	private Object child;
	
	public void set(int ver){
		child = new BiChildUnTypedPrivate(this, "set" + ver);
	}
	
	public boolean jdk2(){
		return true;
	}
}
