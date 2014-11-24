/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions.propagation;

import com.db4o.ext.*;

public class OneTimeDb4oExceptionPropagationFixture extends OneTimeFatalExceptionPropagationFixtureBase {

	@Override
	protected Class<? extends RuntimeException> exceptionType() {
		return Db4oException.class;
	}

	public void throwInitialException() {
		throw new Db4oException();
	}

	public String label() {
		return "non-fatal/oneDb4o";
	}

}
