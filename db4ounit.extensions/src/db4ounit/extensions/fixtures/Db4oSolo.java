/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.fixtures;

import db4ounit.extensions.*;

public class Db4oSolo extends AbstractFileBasedDb4oFixture {
	
	private static final String FILE = "db4oSoloTest.db4o";

	public Db4oSolo() {
	}
	
	public Db4oSolo(FixtureConfiguration fixtureConfiguration) {
		fixtureConfiguration(fixtureConfiguration);
	}

	public String label() {
		return buildLabel("SOLO");
	}
	
	@Override
	protected String fileName() {
		return FILE;
	}
}
