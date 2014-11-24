/* Copyright (C) 2004 - 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.*;
import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class CallConstructorsConfigTestCase extends StandaloneCSTestCaseBase {
	
	protected void runTest() throws Throwable {
		withClient(new ContainerBlock() {
			public void run(ObjectContainer client) {	
				client.store(new Item());
			}
		});
		
		withClient(new ContainerBlock() {
			public void run(ObjectContainer client) {	
				Assert.areEqual(1, client.query(Item.class).size());
			}
		});
	}

	protected void configure(final Configuration config) {
		config.callConstructors(true);
		config.exceptionsOnNotStorable(true);
	}

}
