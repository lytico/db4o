/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

/**
 * 
 */
package com.db4o.drs.test.versant;

/**
* @exclude
*/
public class VodEvent {
	
	private final String _name;

	private VodEvent(String name) {
		_name = name;
	}

	public static final VodEvent CREATED = new VodEvent("created");
	
	public static final VodEvent MODIFIED = new VodEvent("modified");
	
	public static final VodEvent DELETED = new VodEvent("deleted");
	
	@Override
	public String toString() {
		return "VersantEvent: " + _name;
	}
	
}