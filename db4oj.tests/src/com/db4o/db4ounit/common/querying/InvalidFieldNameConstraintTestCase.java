package com.db4o.db4ounit.common.querying;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class InvalidFieldNameConstraintTestCase extends AbstractDb4oTestCase {

	public static class Person {
		public String _firstName;
		public String _lastName;
		
		public Person(String firstName, String lastName) {
			_firstName = firstName;
			_lastName = lastName;
		}
	}

	@Override
	protected void configure(Configuration config) throws Exception {
		config.blockSize(8);
		config.objectClass(Person.class).objectField("_firstName").indexed(true);
		config.objectClass(Person.class).objectField("_lastName").indexed(true);
		config.add(new TransparentActivationSupport());
		
	}
	
	@Override
	protected void store() throws Exception {
		store(new Person("John", "Doe"));
	}

	public void testQuery() {
		Query query = newQuery(Person.class);
		query.descend("_nonExistent").constrain("X");
		ObjectSet<Person> result = query.execute();
		Assert.areEqual(0, result.size());
	}
}
