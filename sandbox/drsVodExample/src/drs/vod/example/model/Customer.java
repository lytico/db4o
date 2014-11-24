/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package drs.vod.example.model;

public class Customer {
	
	private String _name;
	
	public Customer(){
		
	}
	
	public Customer(String name) {
		_name = name;
	}

	@Override
	public String toString() {
		return "Customer [_name=" + _name + "]";
	}

}
