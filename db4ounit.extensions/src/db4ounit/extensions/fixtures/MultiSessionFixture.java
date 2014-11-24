/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package db4ounit.extensions.fixtures;

import com.db4o.ext.*;

import db4ounit.extensions.*;

public interface MultiSessionFixture extends Db4oFixture {
	ExtObjectContainer openNewSession(Db4oTestCase testInstance) throws Exception;
}
