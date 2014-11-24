/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package db4ounit.extensions;

import db4ounit.extensions.fixtures.*;

public class Db4oClientServerTestCase extends AbstractDb4oTestCase implements OptOutSolo {
	
	public Db4oClientServerFixture clientServerFixture() {
		return (Db4oClientServerFixture) fixture();
	}
	
}
