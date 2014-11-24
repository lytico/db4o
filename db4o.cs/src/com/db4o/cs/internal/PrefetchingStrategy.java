/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.cs.internal;

import com.db4o.foundation.*;
import com.db4o.internal.*;


/**
 * Defines a strategy on how to prefetch objects from the server.
 */
public interface PrefetchingStrategy {

	int prefetchObjects(ClientObjectContainer container, Transaction trans, IntIterator4 ids, Object[] prefetched, int prefetchCount);

}
