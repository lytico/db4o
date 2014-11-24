/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.qlin;

import com.db4o.*;
import com.db4o.qlin.*;
import com.db4o.query.*;

/**
 * @exclude
 */
public abstract class QLinSubNode<T> extends QLinSodaNode<T>{
	
	protected final QLinRoot<T> _root;

	public QLinSubNode(QLinRoot<T> root) {
		_root = root;
	}
	
	protected QLinRoot<T> root(){
		return _root;
	}
	
	protected Query query(){
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
