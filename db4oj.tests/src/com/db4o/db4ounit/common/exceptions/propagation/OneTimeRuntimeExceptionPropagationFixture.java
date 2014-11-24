/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions.propagation;


public class OneTimeRuntimeExceptionPropagationFixture extends OneTimeFatalExceptionPropagationFixtureBase {

	@Override
	protected Class<? extends RuntimeException> exceptionType() {
		return RuntimeException.class;
	}

	public void throwInitialException() {
		throw new RuntimeException();
	}

	public String label() {
		return "non-fatal/oneRTE";
	}

}
