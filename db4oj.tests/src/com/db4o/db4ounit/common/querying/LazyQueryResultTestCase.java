/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.internal.query.result.*;


public class LazyQueryResultTestCase extends QueryResultTestCase {

	public static void main(String[] args) {
		new LazyQueryResultTestCase().runSolo();
	}

	protected AbstractQueryResult newQueryResult() {
		return new LazyQueryResult(trans());
	}

}
