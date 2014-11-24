/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.mapping;

/**
 * @exclude
 */
public class MappingNotFoundException extends RuntimeException {

	private static final long serialVersionUID = -1771324770287654802L;
	
	private int _id;
	
	public MappingNotFoundException(int id) {
		this._id = id;
	}

	public int id() {
		return _id;
	}
	
	public String toString() {
		return super.toString()+" : "+_id;
	}
}
