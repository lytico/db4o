package com.db4o.db4ounit.common.fieldindex;

import com.db4o.config.*;
import com.db4o.diagnostic.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class UseSecondBestIndexTestCase extends AbstractDb4oTestCase {

	boolean loadedFromClassIndex;

	@Override
	protected void configure(Configuration config) throws Exception {
		config.objectClass(Parent.class).objectField("id").indexed(true);
		config.objectClass(Child.class).objectField("id").indexed(true);
		config.diagnostic().addListener(new DiagnosticListener() {
			public void onDiagnostic(Diagnostic d) {
				if (d instanceof LoadedFromClassIndex) {
					loadedFromClassIndex = true;
				}
			}
		});

	}

	public static class Parent {

		public Child child;

		public int id;

		public Parent(int id) {
			this.id = id;
		}

	}

	public static class Child {

		public int id;

		public Child(int id) {
			this.id = id;
		}

	}

	@Override
	protected void store() throws Exception {
		store(new Parent(42));
		Parent parent2 = new Parent(42);
		parent2.child = new Child(42);
		store(parent2);
	}

	public void testUsingIndex() {
		Query q = db().query();
		q.constrain(Parent.class);
		q.descend("id").constrain(42);
		q.descend("child").descend("id").constrain(42);
		q.execute();
		Assert.isFalse(loadedFromClassIndex);

	}

}