package com.db4o.instrumentation.classfilter;

import com.db4o.instrumentation.core.*;

/**
 * A class filter composed of multiple other filters - accepts if one of the filters accepts.
 */
public class CompositeOrClassFilter implements ClassFilter {

	private final ClassFilter[] _filters;
	
	public CompositeOrClassFilter(ClassFilter[] filters) {
		_filters = filters;
	}

	public boolean accept(Class clazz) {
		for (int i = 0; i < _filters.length; i++) {
			if(_filters[i].accept(clazz)) {
				return true;
			}
		}
		return false;
	}

}
