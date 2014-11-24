/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions.propagation;


public class RecurringRuntimeExceptionPropagationFixture extends RecurringFatalExceptionPropagationFixtureBase {

	public void throwInitialException() {
		throw new RuntimeException(INITIAL_MESSAGE);
	}

	public void throwCloseException() {
		throw new RuntimeException(CLOSE_MESSAGE);
	}

	public String label() {
		return "fatal/recRTE";
	}

	@Override
	protected Class<? extends RuntimeException> exceptionType() {
		return RuntimeException.class;
	}
}
