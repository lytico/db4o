/**
 * 
 */
package com.db4o.junit.launcher;

import java.io.PrintWriter;

import com.db4o.foundation.Iterable4;

import db4ounit.*;

public final class VerboseConsoleTestRunner extends ConsoleTestRunner {
	private final PrintWriter out;

	@SuppressWarnings("unchecked")
	VerboseConsoleTestRunner(Iterable4 suite, PrintWriter out) {
		super(suite);
		this.out = out;
	}

	protected db4ounit.TestResult createTestResult() {
		return new TestResult() {
			@Override
			public void testFailed(Test test, Throwable failure) {
				super.testFailed(test, failure);
				failure.printStackTrace(System.err);
				failure.printStackTrace(out);
			}
		};
	}
}