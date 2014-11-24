/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

public class TreeStringObject<T> extends TreeString {

	public final T _value;

	public TreeStringObject(String key, T value) {
		super(key);
		this._value = value;
	}

	public Object shallowClone() {
		TreeStringObject tso = new TreeStringObject<T>(_key, _value);
		return shallowCloneInternal(tso);
	}
}