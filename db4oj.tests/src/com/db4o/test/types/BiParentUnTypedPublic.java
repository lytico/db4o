/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class BiParentUnTypedPublic extends RTest
{
	public Object child;
	
	public void set(int ver){
		child = new BiChildUnTypedPublic();
		((BiChildUnTypedPublic)child).parent = this;
		((BiChildUnTypedPublic)child).name = "set" + ver;
	}
}
