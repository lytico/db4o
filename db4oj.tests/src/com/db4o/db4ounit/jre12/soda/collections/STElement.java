/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.soda.collections;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class STElement {
	
	public String foo1;
	public Object foo2;
	
	public STElement(){
	}
	
	public STElement(String foo1, Object foo2) {
		this.foo1 = foo1;
		this.foo2 = foo2;
	}

}

