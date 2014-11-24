/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;
import com.db4o.marshall.*;


/**
 * Interface for comparison support in queries.
 */
public interface Comparable4<T> {
	
	/**
	 * creates a prepared comparison to compare multiple objects
	 * against one single object.
	 * @param context the context of the comparison 
	 * @param obj the object that is to be compared 
	 * against multiple other objects
	 * @return the prepared comparison
	 */
	PreparedComparison prepareComparison(Context context, T obj);
	
}

