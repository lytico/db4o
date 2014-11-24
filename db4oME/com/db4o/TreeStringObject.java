/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public class TreeStringObject extends TreeString {

	public final Object _object;

	public TreeStringObject(String a_key, Object a_object) {
		super(a_key);
		this._object = a_object;
	}

	public Object shallowClone() {
		TreeStringObject tso = new TreeStringObject(_key, _object);
		return shallowCloneInternal(tso);
	}
}