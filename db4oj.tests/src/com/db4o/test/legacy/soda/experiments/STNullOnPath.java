/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.experiments;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;


public class STNullOnPath implements STClass {

	public static transient SodaTest st;

	Boolean bool;

	public STNullOnPath() {
	}

	public STNullOnPath(Boolean bool) {
		this.bool = bool;
	}

	public Object[] store() {
		return new Object[] {
			new STNullOnPath(new Boolean(false))
			};
	}

	public void test() {
		Query q = st.query();
		q.constrain(new STNullOnPath());
		q.descend("bool").constrain(null);
		Object[] r1 = store();
		st.expectNone(q);
	}
}
