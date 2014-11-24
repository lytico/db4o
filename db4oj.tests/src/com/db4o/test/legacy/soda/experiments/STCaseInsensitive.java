/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.experiments;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STCaseInsensitive implements STClass {

	public static transient SodaTest st;
	
	String str;
	
	
	public STCaseInsensitive() {
	}
	
	public STCaseInsensitive(String str) {
		this.str = str;
	}

	public Object[] store() {
		return new Object[] {
			new STCaseInsensitive("Hihoho"),
			new STCaseInsensitive("Hello"),
			new STCaseInsensitive("hello")
		};
	}

	public void test() {
		Query q = st.query();
		q.constrain(STCaseInsensitive.class);
		q.descend("str").constrain(new Evaluation() {
            public void evaluate(Candidate candidate) {
                candidate.include(candidate.getObject().toString().toLowerCase().startsWith("hell"));
            }
        });
		Object[] r = store();
		st.expect(q, new Object[] { r[1], r[2] });
	}

}

