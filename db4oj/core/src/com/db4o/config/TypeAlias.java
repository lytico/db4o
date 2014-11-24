/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.config;

import com.db4o.internal.*;

/**
 * a simple Alias for a single Class or Type, using equals on
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
    
    public TypeAlias(Class storedClass, Class runtimeClass){
    	this(ReflectPlatform.fullyQualifiedName(storedClass), ReflectPlatform.fullyQualifiedName(runtimeClass));
    }

	/**
     * returns the stored type name if the alias was written for the passed runtime type name  
	 */
    public String resolveRuntimeName(String runtimeTypeName) {
		return _runtimeType.equals(runtimeTypeName)
			? _storedType
			: null;
	}
    
	/**
     * returns the runtime type name if the alias was written for the passed stored type name  
	 */
	public String resolveStoredName(String storedTypeName) {
		return _storedType.equals(storedTypeName)
		? _runtimeType
		: null;
	}

}
