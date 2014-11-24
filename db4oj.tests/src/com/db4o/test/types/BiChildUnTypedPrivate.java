/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class BiChildUnTypedPrivate
{
	private Object parent;
	private String name;
	
	public BiChildUnTypedPrivate(){
	}
	
	public BiChildUnTypedPrivate(Object a_parent, String a_name){
		parent = a_parent;
		name = a_name;
	}

}
