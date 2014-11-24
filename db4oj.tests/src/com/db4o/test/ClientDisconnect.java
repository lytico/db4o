/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import java.io.*;

import com.db4o.cs.internal.*;
import com.db4o.ext.*;

public class ClientDisconnect {
	public void testDisconnectThenGet() throws IOException {
		if(!Test.isClientServer()) {
			return;
		}
		ExtClient client=(ExtClient)Test.objectContainer();
		((ClientObjectContainer)client).socket().close();
		try {
			client.queryByExample(null);
			Test.error("expected exception on get after close");
		}
		catch(Db4oException exc) {
			// OK, expected
		}
		Test.close();
	}
	
	public static void main(String[] args) {
		Test.run(ClientDisconnect.class);
	}
}
