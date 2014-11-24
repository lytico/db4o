/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.cobra.qlin;

import com.db4o.*;
import com.db4o.qlin.*;
import com.versant.odbms.query.*;

public abstract class CLinSubNode<T> extends CLinCobraNode<T>{
	
	protected final CLinRoot<T> _root;

	public CLinSubNode(CLinRoot<T> root) {
		_root = root;
	}
	
	protected CLinRoot<T> root(){
		return _root;
	}
	
	protected DatastoreQuery query(){
		return root().query();
	}
	
	public QLin<T> limit(int size){
		root().limit(size);
		return this;
	}
	
	public ObjectSet<T> select() {
		return root().select();
	}
	
}
