/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * a prepared comparison, to compare multiple objects 
 * with one single object. 
 */
public interface PreparedComparison<T> {
	
	/**
	 * return a negative int, zero or a positive int if
	 * the object being held in 'this' is smaller, equal 
	 * or greater than the passed object.<br><br>
	 * 
	 * Typical implementation: return this.object - obj;
	 */
	public int compareTo(T obj);

}
