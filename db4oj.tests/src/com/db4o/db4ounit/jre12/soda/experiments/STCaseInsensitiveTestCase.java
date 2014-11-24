/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.soda.experiments;
import com.db4o.query.*;



/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class STCaseInsensitiveTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase {

	String str;
	
	
	public STCaseInsensitiveTestCase() {
	}
	
	public STCaseInsensitiveTestCase(String str) {
		this.str = str;
	}

	public Object[] createData() {
		return new Object[] {
			new STCaseInsensitiveTestCase("Hihoho"),
			new STCaseInsensitiveTestCase("Hello"),
			new STCaseInsensitiveTestCase("hello")
		};
	}

	public void test() {
		Query q = newQuery();
		q.constrain(STCaseInsensitiveTestCase.class);
		q.descend("str").constrain(new Evaluation() {
            public void evaluate(Candidate candidate) {
                candidate.include(candidate.getObject().toString().toLowerCase().startsWith("hell"));
            }
        });
		
		expect(q, new int[] { 1, 2 });
	}

}

