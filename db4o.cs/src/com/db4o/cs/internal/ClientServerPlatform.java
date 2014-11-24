/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.cs.internal;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.query.result.*;

/**
 * Platform specific defaults.
 */
public class ClientServerPlatform {

	/**
	 * The default {@link ClientQueryResultIterator} for this platform.
	 * 
	 * @return
	 */
	public static Iterator4 createClientQueryResultIterator(AbstractQueryResult result) {
		final QueryResultIteratorFactory factory = result.config().queryResultIteratorFactory();
		if (null != factory) return factory.newInstance(result);
		return new ClientQueryResultIterator(result);
	}

}
