/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.experiments;
import com.db4o.query.*;


public class STNullOnPathTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase {

	public Boolean bool;

	public STNullOnPathTestCase() {
	}

	public STNullOnPathTestCase(Boolean bool) {
		this.bool = bool;
	}

	public Object[] createData() {
		return new Object[] {
			new STNullOnPathTestCase(new Boolean(false))
			};
	}

	public void test() {
		Query q = newQuery();
		q.constrain(new STNullOnPathTestCase());
		q.descend("bool").constrain(null);
		
		expect(q, new int[] {});
	}
}
