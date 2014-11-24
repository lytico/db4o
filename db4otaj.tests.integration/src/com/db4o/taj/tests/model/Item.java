/* Copyright (C) 2009  db4objects Inc.  http://www.db4o.com */

package com.db4o.taj.tests.model;

public class Item {

	private String _name;
	
	public Item(String name) {
		_name = name;
	}
	
	public String name() {
		return _name;
	}

	public String toString() {
		return "Item: " + _name;
	}
	
}
