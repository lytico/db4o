/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions.propagation;

import com.db4o.ext.*;

public class RecurringDb4oExceptionPropagationFixture extends RecurringFatalExceptionPropagationFixtureBase {

	public void throwInitialException() {
		throw new Db4oException(INITIAL_MESSAGE);
	}

	public void throwCloseException() {
		throw new Db4oException(CLOSE_MESSAGE);
	}

	public String label() {
		return "fatal/recDb4o";
	}

	@Override
	protected Class<? extends RuntimeException> exceptionType() {
		return Db4oException.class;
	}

}
