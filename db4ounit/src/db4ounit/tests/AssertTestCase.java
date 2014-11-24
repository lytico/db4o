package db4ounit.tests;

import db4ounit.Assert;
import db4ounit.AssertionException;
import db4ounit.CodeBlock;
import db4ounit.TestCase;

public class AssertTestCase implements TestCase {
	public void testAreEqual() {
		Assert.areEqual(true, true);
		Assert.areEqual(42, 42);
		Assert.areEqual(new Integer(42), new Integer(42));
		Assert.areEqual(null, null);
		expectFailure(new CodeBlock() {
			public void run() throws Throwable {
				Assert.areEqual(true, false);
			}
		});
		expectFailure(new CodeBlock() {
			public void run() throws Throwable {
				Assert.areEqual(42, 43);
			}
		});
		expectFailure(new CodeBlock() {
			public void run() throws Throwable {
				Assert.areEqual(new Object(), new Object());
			}
		});
		expectFailure(new CodeBlock() {
			public void run() throws Throwable {
				Assert.areEqual(null, new Object());
			}
		});
	}	
	
	public void testAreSame() {
		expectFailure(new CodeBlock() {
			public void run() throws Throwable {
				Assert.areSame(new Object(), new Object());
			}
		});
		Assert.areSame(this, this);
	}
	
	private void expectFailure(CodeBlock block) {
		Assert.expect(AssertionException.class, block);
	}
}
