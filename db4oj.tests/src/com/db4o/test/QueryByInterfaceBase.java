/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

public abstract class QueryByInterfaceBase {
	
	private static class SimpleEvaluation implements Evaluation {
		private String value;
		
		private SimpleEvaluation(String value) {
			this.value = value;
		}

		public void evaluate(Candidate candidate) {
			candidate.include(((IFoo)candidate.getObject()).s().equals(value));
		}
	}

	public static interface IFoo {
		String s();
	}
	
	public static class Bar implements IFoo {
		public int i;

		public Bar(int i) {
			this.i = i;
		}

		public String s() {
			return String.valueOf((char)('A'+i));
		}
	}

	public static class Baz implements IFoo {
		public String s;

		public Baz(String s) {
			this.s = s;
		}

		public String s() {
			return s;
		}
	}

	protected void assertSODA(String value,int expCount) {
		Query query=Test.objectContainer().query();
		Constraint constraint=query.constrain(IFoo.class);
		Test.ensure(constraint!=null);
		query.descend("s").constrain(value);
		ObjectSet result=query.execute();
		Test.ensure(result.size()==expCount);
	}

	protected void assertEvaluation(String value,int expCount) {
		Query query=Test.objectContainer().query();
		Constraint constraint=query.constrain(IFoo.class);
		Test.ensure(constraint!=null);
		query.constrain(new SimpleEvaluation(value));
		ObjectSet result=query.execute();
		Test.ensure(result.size()==expCount);
	}
}
