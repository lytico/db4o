/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ParameterizedEvaluationTestCase extends Db4oClientServerTestCase {
	
	public static void main(String[] args) {
		new ParameterizedEvaluationTestCase().runConcurrency();
	}

	public String str;

	protected void store() {
		store("one");
		store("fun");
		store("ton");
		store("sun");
	}

	private void store(String str) {
		ParameterizedEvaluationTestCase pe = new ParameterizedEvaluationTestCase();
		pe.str = str;
		store(pe);
	}

	public void conc(ExtObjectContainer oc) {
		Assert.areEqual(2, queryContains(oc, "un").size());
	}

	private ObjectSet queryContains(ExtObjectContainer oc, final String str) {
		Query q = oc.query();
		q.constrain(ParameterizedEvaluationTestCase.class);
		q.constrain(new MyEvaluation(str));
		return q.execute();
	}
	
	public static class MyEvaluation implements Evaluation {
		public String str;
		public MyEvaluation(String str) {
			this.str = str;
		}
		public void evaluate(Candidate candidate) {
			ParameterizedEvaluationTestCase pe = (ParameterizedEvaluationTestCase) candidate
					.getObject();
			boolean inc = pe.str.indexOf(str) != -1;
			candidate.include(inc);
		}
	}

}
