/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

/**
 * @exclude
 */
public class RawClassSpec {

	private final String _name;
	private final int _superClassID;
	private final int _numFields;

	public RawClassSpec(final String name, final int superClassID, final int numFields) {
		_name = name;
		_superClassID = superClassID;
		_numFields = numFields;
	}
	
	public String name() {
		return _name;
	}
	
	public int superClassID() {
		return _superClassID;
	}
	
	public int numFields() {
		return _numFields;
	}
}
