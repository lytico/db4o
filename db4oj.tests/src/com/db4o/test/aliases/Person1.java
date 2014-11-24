/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.aliases;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class Person1 {

	private String _name;

	public Person1(String name) {
		_name = name;
	}
	
	public String name() {
		return _name;
	}

}
