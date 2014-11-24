package com.db4o.db4ounit.common.refactor;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class ReAddFieldTestCase extends AbstractDb4oTestCase implements OptOutDefragSolo {
	
	public static class Version1 {
		public String name;
		public int id;
		
		public Version1(String name, int id) {
			this.name = name;
			this.id = id;
		}
		
		public Version1() {
		}
	}
	
	public static class Version2 {
		public int id;
	}
	
	@Override
	protected void store() throws Exception {
		store(new Version1("ltuae", 42));
	}
	
	public void test() throws Exception {
		final TypeAlias alias = new TypeAlias(Version1.class, Version2.class); 
		
		fixture().config().addAlias(alias);
		reopen();
		
		Assert.areEqual(42, retrieveOnlyInstance(Version2.class).id);
		
		fixture().config().removeAlias(alias);
		reopen();
		
		final Version1 original = retrieveOnlyInstance(Version1.class);
		Assert.areEqual("ltuae", original.name);
		Assert.areEqual(42, original.id);
		
	}
}
