/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.instrumentation.classfilter;

import com.db4o.instrumentation.core.*;
import com.db4o.instrumentation.util.*;

/**
 * Accepts all classes that are not part of the Java platform.
 */
public class AcceptAllClassesFilter implements ClassFilter {

	public boolean accept(Class clazz) {
		return !BloatUtil.isPlatformClassName(clazz.getName());
	}

}
