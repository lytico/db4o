/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.bench.crud;

public class Item {
	
	private static final String LOAD = "LOAD__________________________";
	
	private static final String UPDATE = "ccccc";
	
	
	public String _string;
	
	
	public Item(String str){
		_string = str;
	}

	public static Object newItem(int i) {
		return new Item(LOAD + i);
	}

	public void change() {
		_string = _string + UPDATE;
	}

}
