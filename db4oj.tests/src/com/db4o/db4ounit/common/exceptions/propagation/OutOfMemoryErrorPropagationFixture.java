/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions.propagation;

import db4ounit.*;

public class OutOfMemoryErrorPropagationFixture implements ExceptionPropagationFixture {

	public void throwInitialException() {
		throw new OutOfMemoryError();
	}

	public void throwShutdownException() {
		Assert.fail();
	}

	public void throwCloseException() {
		Assert.fail();
	}

	public void assertExecute(final DatabaseContext context, final TopLevelOperation op) {
		Assert.expect(OutOfMemoryError.class, new CodeBlock() {
			public void run() throws Throwable {
				op.apply(context);
			}
		});
		Assert.isFalse(context.storageIsClosed());
	}

	public String label() {
		return "OOME";
	}

}
