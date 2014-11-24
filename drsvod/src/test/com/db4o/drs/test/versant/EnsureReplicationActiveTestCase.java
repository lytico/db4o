/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;

import com.db4o.drs.test.versant.data.*;

import db4ounit.*;

public class EnsureReplicationActiveTestCase extends VodProviderTestCaseBase {
	
	public void testThrowIfNoActiveReplication(){
		Assert.expect(IllegalStateException.class,new CodeBlock() {
			public void run() throws Throwable {
				_provider.objectsChangedSinceLastReplication();
			}
		});
		
		Assert.expect(IllegalStateException.class,new CodeBlock() {
			public void run() throws Throwable {
				_provider.objectsChangedSinceLastReplication(Item.class);
			}
		});
		
		Assert.expect(IllegalStateException.class,new CodeBlock() {
			public void run() throws Throwable {
				_provider.getLastReplicationVersion();
			}
		});


	}

}
