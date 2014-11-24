/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.config.annotations.*;



/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class Sheep {

	@Indexed 
	private String name;
	private boolean constructorCalled=false;

	Sheep parent;

	public Sheep(String name, Sheep parent) {
		this.name = name;
		this.parent = parent;
		constructorCalled=true;
	}

	public boolean constructorCalled() {
		return constructorCalled;
	}
	
	@Override 
	public String toString() {
		return name + " " + parent;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}
}
