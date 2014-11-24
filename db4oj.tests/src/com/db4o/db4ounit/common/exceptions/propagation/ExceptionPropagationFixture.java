/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions.propagation;

import db4ounit.fixtures.*;

public interface ExceptionPropagationFixture extends Labeled {

	void throwInitialException();

	void throwShutdownException();

	void throwCloseException();
	
	void assertExecute(DatabaseContext context, TopLevelOperation op);

}
