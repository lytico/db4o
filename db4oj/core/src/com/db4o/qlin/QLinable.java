/* Copyright (C) 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.qlin;

/**
 * support for the new experimental QLin ("Coolin") query interface.
 * We would really like to have LINQ for Java instead. 
 * @since 8.0
 */
public interface QLinable {
	
	/**
	 * starts a {@link QLin} query against a class. 
	 */
	public <T> QLin<T> from(Class<T> clazz);

}
