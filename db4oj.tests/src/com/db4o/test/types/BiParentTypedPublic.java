/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class BiParentTypedPublic extends RTest
{
	public BiChildTypedPublic child;

	public void set(int ver){
		child = new BiChildTypedPublic();
		child.parent = this;
		if(ver == 1){
			child.name = "set1";
		}else{
			child.name = "set2";
		}
	}
}
