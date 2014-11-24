package db4ounit.tests;

import db4ounit.*;

public class ReinstantiatePerMethodTest implements TestCase {

	private int a = 0;

	public void test1() {
		Assert.areEqual(0,a);
		a = 1;
	}

	public void test2() {
		Assert.areEqual(0,a);
		a = 2;
	}
}
