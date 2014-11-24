/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.reflect.generic;

/**
 * @exclude
 */
public interface GenericConverter {
	
	public String toString(GenericObject obj);

	public String toString(GenericArray array);

}
