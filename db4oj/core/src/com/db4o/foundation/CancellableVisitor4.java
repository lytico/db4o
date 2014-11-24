/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

public interface CancellableVisitor4<T> {
	
	/**
	 * @return true to continue traversal, false otherwise
	 */
	public boolean visit(T obj); 

}
