/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package db4ounit.extensions.tests;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class Db4oEmbeddedSessionFixtureTestCase implements TestCase {
	
	final Db4oEmbeddedSessionFixture subject = new Db4oEmbeddedSessionFixture();
	
	public void testDoesNotAcceptRegularTest() {
		Assert.isFalse(subject.accept(RegularTest.class));
	}
	
	public void testAcceptsDb4oTest() {
		Assert.isTrue(subject.accept(Db4oTest.class));
	}
	
	public void testDoesNotAcceptOptOutCS() {
		Assert.isFalse(subject.accept(OptOutTest.class));
	}
	
	public void testDoesNotAcceptOptOutAllButNetworkingCS() {
		Assert.isFalse(subject.accept(OptOutAllButNetworkingCSTest.class));
	}
	
	public void testAcceptsOptOutNetworking() {
		Assert.isTrue(subject.accept(OptOutNetworkingTest.class));
	}
	
	static class RegularTest implements TestCase {
	}
	
	static class Db4oTest implements Db4oTestCase {
	}
	
	static class OptOutTest implements OptOutMultiSession {
	}
	
	static class OptOutNetworkingTest implements OptOutNetworkingCS {
	}
	
	static class OptOutAllButNetworkingCSTest implements OptOutAllButNetworkingCS {
		
	}
	

}
