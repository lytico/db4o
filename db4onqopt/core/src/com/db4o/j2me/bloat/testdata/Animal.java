/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.j2me.bloat.testdata;

public /*abstract*/ class Animal implements Being {
	private String _name;


	protected Animal(String name) {
		_name = name;
	}

	public String name() {
		return _name;
	}
}
