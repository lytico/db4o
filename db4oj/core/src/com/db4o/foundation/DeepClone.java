/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

/**
 * Deep clone
 * @exclude
 **/
public interface DeepClone {

	/** The parameter allows passing one new object so parent
	  * references can be corrected on children.*/
    Object deepClone(Object context);

}
