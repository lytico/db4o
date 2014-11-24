/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.typehandlers;

import com.db4o.reflect.*;

/**
 * allows installing a Typehandler for a single classname.
 */
public final class SingleNamedClassTypeHandlerPredicate implements TypeHandlerPredicate {
    
	private final String _className;
	
	public SingleNamedClassTypeHandlerPredicate(String className) {
		_className = className;
	}
	
	public boolean match(ReflectClass candidate) {
		return candidate.getName().equals(_className);
	}
}