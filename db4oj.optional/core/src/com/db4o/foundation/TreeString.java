/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

import com.db4o.internal.*;
import com.db4o.internal.handlers.*;

public class TreeString extends Tree<String> {
	
	public String _key;
	
	public TreeString(String key) {
		this._key = key;
	}
	
	protected Tree shallowCloneInternal(Tree tree) {
		TreeString ts = (TreeString) super.shallowCloneInternal(tree);
		ts._key = _key;
		return ts;
	}
	
	public Object shallowClone() {
		return shallowCloneInternal(new TreeString(_key));
	}
	
	public int compare(Tree to) {
		return StringHandler
			.compare(
				Const4.stringIO.write(_key),
				Const4.stringIO.write(((TreeString) to)._key));
	}
	
	public String key(){
		return _key;
	}
	
}