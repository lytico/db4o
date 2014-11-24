/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.config;

import com.db4o.*;

/**
 * Assigns a fixed, pre-defined name to the given {@link ObjectContainer}.
 */
public class SimpleNameProvider implements NameProvider {

	private final String _name;
	
	public SimpleNameProvider(String name) {
		_name = name;
	}
	
	public String name(ObjectContainer db) {
		return _name;
	}

}
