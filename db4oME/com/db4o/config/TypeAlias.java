/* Copyright (C) 2004 - 2006  db4objects Inc.  http://www.db4o.com */

package com.db4o.config;

/**
 * a simple Alias for a single Class or Type, using #equals() on
 * the names in the resolve method.
 * <br><br>See {@link Alias} for concrete examples.  
 */
public class TypeAlias implements Alias {

	private final String _storedType;
    
	private final String _runtimeType;

	/**
     * pass the stored name as the first 
     * parameter and the desired runtime name as the second parameter. 
	 */
    public TypeAlias(String storedType, String runtimeType) {
		if (null == storedType || null == runtimeType) throw new IllegalArgumentException();
		_storedType = storedType;
		_runtimeType = runtimeType;
	}

	/**
     * checking if both names are equal. 
	 */
    public String resolve(String runtimeType) {
		return _runtimeType.equals(runtimeType)
			? _storedType
			: null;
	}

}
