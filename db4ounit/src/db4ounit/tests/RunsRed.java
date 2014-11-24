package db4ounit.tests;

import db4ounit.FailingTest;

class RunsRed extends FailingTest {
	public RunsRed(RuntimeException exception) {
		super("RunsRed", exception);
	}
}