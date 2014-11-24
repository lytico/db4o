/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;



/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class SheepNotAnnotated {
	
	private String name;
	private boolean constructorCalled=false;
	
	SheepNotAnnotated parent;
	public SheepNotAnnotated(String name, SheepNotAnnotated parent) {
		this.name = name;
		this.parent = parent;
		constructorCalled=true;
	}
@Override
public String toString() {
	return name+ " "+ parent;
}
public String getName() {
	return name;
}
public void setName(String name) {
	this.name = name;
}

	public boolean constructorCalled() {
		return constructorCalled;
	}
}
