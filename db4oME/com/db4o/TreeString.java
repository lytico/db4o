/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public class TreeString extends Tree {

	public String _key;

	public TreeString(String a_key) {
		this._key = a_key;
	}

	protected Tree shallowCloneInternal(Tree tree) {
		TreeString ts = (TreeString) super.shallowCloneInternal(tree);
		ts._key = _key;
		return ts;
	}

	public Object shallowClone() {
		return shallowCloneInternal(new TreeString(_key));
	}

	public int compare(Tree a_to) {
		return YapString
				.compare(YapConst.stringIO.write(((TreeString) a_to)._key),
						YapConst.stringIO.write(_key));
	}

}
