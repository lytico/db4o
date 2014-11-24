/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class BiChildTypedPrivate
{
	@SuppressWarnings("unused")
	private BiParentTypedPrivate parent;
	@SuppressWarnings("unused")
	private String name;
	
	public BiChildTypedPrivate(){
	}
	
	public BiChildTypedPrivate(BiParentTypedPrivate a_parent, String a_name){
		parent = a_parent;
		name = a_name;
	}
}
