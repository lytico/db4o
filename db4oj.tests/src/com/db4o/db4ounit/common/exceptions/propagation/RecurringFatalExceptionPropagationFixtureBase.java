/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions.propagation;

import com.db4o.ext.*;

import db4ounit.*;

public abstract class RecurringFatalExceptionPropagationFixtureBase implements ExceptionPropagationFixture {

	protected static final String CLOSE_MESSAGE = "B";
	protected static final String INITIAL_MESSAGE = "A";

	public void throwShutdownException() {
		Assert.fail();
	}

	public void assertExecute(DatabaseContext context, TopLevelOperation op) {
		try {
			op.apply(context);
			Assert.fail();
		}
		catch(CompositeDb4oException exc) {
			Assert.areEqual(2, exc._exceptions.length);
			assertExceptionMessage(exc, INITIAL_MESSAGE, 0);
			assertExceptionMessage(exc, CLOSE_MESSAGE, 1);
		}
	}

	private void assertExceptionMessage(CompositeDb4oException exc, String expected, int idx) {
		Assert.areEqual(expected, exc._exceptions[idx].getMessage());
	}

	protected abstract Class<? extends RuntimeException> exceptionType();

}