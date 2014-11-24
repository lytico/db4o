/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package db4ounit.extensions.dbmock;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;

/**
 * @sharpen.partial
 */
public class MockServer implements ObjectServer {
	public boolean close() {
		throw new NotImplementedException();
	}

	public ExtObjectServer ext() {
		throw new NotImplementedException();
	}

	public void grantAccess(String userName, String password) {
		throw new NotImplementedException();
	}

	public ObjectContainer openClient() {
		throw new NotImplementedException();
	}
}