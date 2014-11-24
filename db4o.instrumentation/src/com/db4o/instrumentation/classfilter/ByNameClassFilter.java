/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.instrumentation.classfilter;

import com.db4o.instrumentation.core.*;

/**
 * Filter by class name/prefix.
 */

public class ByNameClassFilter implements ClassFilter {

	private final String[] _names;
	private final boolean _prefixMatch;
	
	public ByNameClassFilter(String fullyQualifiedName) {
		this(fullyQualifiedName, false);
	}

	public ByNameClassFilter(String name, boolean prefixMatch) {
		this(new String[]{ name }, prefixMatch);
	}

	public ByNameClassFilter(String[] fullyQualifiedNames) {
		this(fullyQualifiedNames, false);
	}

	public ByNameClassFilter(String[] names, boolean prefixMatch) {
		_names = names;		
		_prefixMatch = prefixMatch;
	}

	public boolean accept(Class clazz) {
		for (int idx = 0; idx < _names.length; idx++) {
			boolean match = (_prefixMatch ? clazz.getName().startsWith(_names[idx]) : _names[idx].equals(clazz.getName()));
			if(match) {
				return true;
			}
		}
		return false;
	}

}
