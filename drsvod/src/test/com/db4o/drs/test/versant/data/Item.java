/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant.data;

public class Item {
	
	private String _name;
	
	public Item() {
	}
	
	public Item(String name){
		_name = name;
	}
	
	public String name(){
		return _name;
	}
	
	@Override
	public boolean equals(Object obj) {
		if(! (obj instanceof Item)){
			return false;
		}
		Item other = (Item) obj;
		if(_name == null){
			return other._name == null;
		}
		return _name.equals(other._name);
	}
	
	public void name(String name){
		_name = name;
	}
	
	@Override
	public String toString() {
		return "Item name:'" + _name + "'";
	}

}
