/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class RecursiveUnTypedPublic extends RTest
{
	public Object recurse;
	public String depth;
	
	public void set(int ver){
		set(ver,10);
	}
	
	private void set(int ver, int a_depth){
		depth = "s" + ver + ":" +  a_depth;	
		if(a_depth > 0){
			recurse = new RecursiveUnTypedPublic();
			((RecursiveUnTypedPublic)recurse).set(ver, a_depth - 1);
		}
	}
}
