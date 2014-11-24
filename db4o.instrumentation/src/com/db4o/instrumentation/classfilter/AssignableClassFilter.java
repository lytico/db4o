/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.instrumentation.classfilter;

import com.db4o.instrumentation.core.*;

/**
 * Filter by superclass/interface.
 */
public class AssignableClassFilter implements ClassFilter {

	private Class _targetClazz;
	
	public AssignableClassFilter(Class targetClazz) {
		_targetClazz = targetClazz;
	}
	
	public boolean accept(Class clazz) {
		return _targetClazz.isAssignableFrom(clazz);
	}

}
