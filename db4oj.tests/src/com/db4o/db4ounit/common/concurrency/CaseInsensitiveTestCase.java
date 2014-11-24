/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * demonstrates a case-insensitive query using an Evaluation
 */
public class CaseInsensitiveTestCase extends Db4oClientServerTestCase {

	public static void main(String[] args) {
		new CaseInsensitiveTestCase().runConcurrency();
	}

	public String name;

	public CaseInsensitiveTestCase() {
	}

	public CaseInsensitiveTestCase(String name) {
		this.name = name;
	}

	protected void store() {
		store(new CaseInsensitiveTestCase("HelloWorld"));
	}

	public void concQueryCaseInsenstive(ExtObjectContainer oc) {
		Query q = oc.query();
		q.constrain(CaseInsensitiveTestCase.class);
		q.constrain(new CaseInsensitiveEvaluation("helloworld"));
		Assert.areEqual(1, q.execute().size());
	}

	public static class CaseInsensitiveEvaluation implements Evaluation {
		public String name;

		public CaseInsensitiveEvaluation(String name) {
			this.name = name;
		}

		public void evaluate(Candidate candidate) {
			CaseInsensitiveTestCase ci = (CaseInsensitiveTestCase) candidate.getObject();
			candidate.include(ci.name.toLowerCase().equals(name.toLowerCase()));
		}

	}

}

