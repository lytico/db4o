/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package db4ounit.extensions;

import db4ounit.*;

public abstract class ComposibleReflectionTestSuite extends ReflectionTestSuite {

	protected Class[] composeTests(Class[] classes) {
		return ComposibleTestSuite.concat(classes, composeWith());
	}
	
	protected Class[] composeWith() {
		return new Class[0];
	}
}