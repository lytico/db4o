/* Copyright (C) 2008   Versant Inc.   http://www.db4o.com */

package db4ounit.extensions;

import db4ounit.fixtures.*;

public final class Db4oFixtureVariable {

	public static final FixtureVariable FIXTURE_VARIABLE = new FixtureVariable("db4o");

	public static Db4oFixture fixture() {
		return (Db4oFixture) FIXTURE_VARIABLE.value();
	}
	
	private Db4oFixtureVariable() {
	}

}
