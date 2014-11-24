/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.query;

import com.db4o.query.*;
 
/**
 * FIXME: Rename to Db4oEnhancedPredicate
 */
public interface Db4oEnhancedFilter {
	void optimizeQuery(Query query);
}
