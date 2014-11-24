/* Copyright (C) 2004 - 2006 Versant Inc.   http://www.db4o.com */
package com.db4o.query;

import java.util.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class JdkComparatorWrapper implements QueryComparator {
	private Comparator _comparator;
	
	public JdkComparatorWrapper(Comparator comparator) {
		this._comparator = comparator;
	}

	public int compare(Object first, Object second) {
		return _comparator.compare(first, second);
	}
}
