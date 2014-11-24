/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions.propagation;

import db4ounit.*;

public abstract class OneTimeFatalExceptionPropagationFixtureBase implements ExceptionPropagationFixture {

	final public void throwShutdownException() {
		Assert.fail();
	}
	
	public void throwCloseException() {
	}
	
	public void assertExecute(final DatabaseContext context, final TopLevelOperation op) {
		Assert.expect(exceptionType(), new CodeBlock() {
			public void run() throws Throwable {
				op.apply(context);
			}
		});
		Assert.isTrue(context.storageIsClosed());
	}

	protected abstract Class<? extends RuntimeException> exceptionType();

}