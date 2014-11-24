/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class BiParentTypedPrivate extends RTest
{
	private BiChildTypedPrivate child;
	
	public void set(int ver){
		child = new BiChildTypedPrivate(this, "set" + ver);
	}
	
	public boolean jdk2(){
		return true;
	}
}
