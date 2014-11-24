/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class QueryNonExistantTestCase extends Db4oClientServerTestCase {
	
	public static void main(String[] args) {
		new QueryNonExistantTestCase().runConcurrency();
	}

	QueryNonExistant1 member;

	public QueryNonExistantTestCase() {
		// db4o constructor
	}

	public QueryNonExistantTestCase(boolean createMembers) {
		member = new QueryNonExistant1();
		member.member = new QueryNonExistant2();
		member.member.member = this;
		// db4o constructor
	}

	public void conc(ExtObjectContainer oc) {
		oc.queryByExample((new QueryNonExistantTestCase(true)));
		assertOccurrences(oc, QueryNonExistantTestCase.class, 0);
		Query q = oc.query();
		q.constrain(new QueryNonExistantTestCase(true));
		Assert.areEqual(0, q.execute().size());
	}

	public static class QueryNonExistant1 {
		QueryNonExistant2 member;
	}

	public static class QueryNonExistant2 extends QueryNonExistant1 {
		QueryNonExistantTestCase member;
	}

}
