/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public interface Visitor4<T> {
    
	public void visit(T obj);
	
}
