/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;


/**
 * @exclude
 */
public class TreeObject extends Tree{
	
	private final Object _object;
	
	private final Comparison4 _function;

	public TreeObject(Object object, Comparison4 function) {
		_object = object;
		_function = function;
	}

	public int compare(Tree tree) {
		return _function.compare(_object, tree.key());
	}

	public Object key() {
		return _object;
	}

}
