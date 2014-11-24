/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.cs.internal.*;
import com.db4o.foundation.*;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class ClientServerTestCaseBase extends Db4oClientServerTestCase implements OptOutAllButNetworkingCS {

	protected ServerMessageDispatcher serverDispatcher() {
		ObjectServerImpl serverImpl = server();
		return (ServerMessageDispatcher)Iterators.next(serverImpl.iterateDispatchers());
	}

	protected ObjectServerImpl server() {
		return (ObjectServerImpl) clientServerFixture().server();
	}
	
	protected ClientObjectContainer client(){
		return (ClientObjectContainer) db();
	}

}
