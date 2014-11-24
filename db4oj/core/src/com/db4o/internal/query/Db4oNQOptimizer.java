/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.query;

import com.db4o.query.*;

public interface Db4oNQOptimizer {
	Object optimize(Query query, Predicate filter);
}
