package com.db4o.db4ounit.common.soda.joins.typed;

import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class JointEqualsIdentityTestCase extends AbstractDb4oTestCase {

	public static class TestSubject {
		public String _name;
		public TestSubject _child;

		public TestSubject(String name, TestSubject child) {
			_name = name;
			_child = child;
		}
	}

	protected void store() throws Exception {
		TestSubject subjectA = new TestSubject("A", null);
		TestSubject subjectB = new TestSubject("B", subjectA);
		TestSubject subjectC = new TestSubject("C", subjectA);
		store(subjectA);
		store(subjectB);
		store(subjectC);
	}
	
	public void testJointEqualsIdentity() {
		TestSubject child = retrieveChild();
		Query query = newQuery(TestSubject.class);
		Constraint constraint = query.descend("_name").constrain("B").equal();
		constraint.and(query.descend("_child").constrain(child).identity());
		Assert.areEqual(1, query.execute().size());
	}

	private TestSubject retrieveChild() {
		Query query = newQuery(TestSubject.class);
		query.descend("_child").constrain(null);
		return (TestSubject) query.execute().next();
	}
}
