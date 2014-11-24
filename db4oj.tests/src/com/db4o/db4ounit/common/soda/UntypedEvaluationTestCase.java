/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.soda;

import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class UntypedEvaluationTestCase extends AbstractDb4oTestCase {

	private final static Class EXTENT = Object.class; // replace with Data.class -> green
	
	public static class Data {
		public int _id;

		public Data(int id) {
			_id = id;
		}
	}

	public static class UntypedEvaluation implements Evaluation {
		public boolean _value;
		
		public UntypedEvaluation(boolean value) {
			_value = value;
		}

		public void evaluate(Candidate candidate) {
			candidate.include(_value);
		}
	}

	protected void store() throws Exception {
		store(new Data(42));
	}

	public void testUntypedRaw() {
		Query query = newQuery(EXTENT); 
		Assert.areEqual(1, query.execute().size());
	}

	public void testUntypedEvaluationNone() {
		Query query = newQuery(EXTENT);
		query.constrain(new UntypedEvaluation(false));
		Assert.areEqual(0, query.execute().size());
	}

	public void testUntypedEvaluationAll() {
		Query query = newQuery(EXTENT);
		query.constrain(new UntypedEvaluation(true));
		Assert.areEqual(1, query.execute().size());
	}

}
