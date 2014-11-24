/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;

import com.db4o.drs.test.versant.jdo.reflect.*;

import db4ounit.*;

public class VodStandaloneTests extends ReflectionTestSuite {
	
	public static void main(String[] args) {
		new VodStandaloneTests().run();
	}

	protected Class[] testCases() {
		
		if(true){
			return new Class[] {
					com.db4o.drs.test.versant.eventlistener.AllTests.class,
					ConcurrentReplicationTestCase.class,
					ConcurrentUpdateTestCase.class,
					EnsureReplicationActiveTestCase.class,
					ExistingJDOFactoryTestCase.class,
					ExplicitListenTestCase.class,
					JdoClassTestCase.class,
					ListenForClassesTestCase.class,
					TriangleReplicationTestCase.class,
					UuidConverterTestCase.class,
					VodDatabaseLifecycleTestCase.class,
					VodDatabaseTestCase.class,
					VodEventTestCase.class,
					VodJviTestCase.class,
					VodProviderTestCase.class,
					VodSimpleObjectContainerTestCase.class,
					ReplicationTimeoutTestCase.class,
					ReplicationViaIntermediateServerTestCase.class,
					PortSetupTestCases.class,
				};
			
		}
		
		return new Class[] {
			com.db4o.drs.test.versant.eventlistener.AllTests.class,
			ConcurrentReplicationTestCase.class,
			ConcurrentUpdateTestCase.class,
			EnsureReplicationActiveTestCase.class,
			ExistingJDOFactoryTestCase.class,
			ExplicitListenTestCase.class,
			JdoClassTestCase.class,
			ListenForClassesTestCase.class,
			TriangleReplicationTestCase.class,
			UuidConverterTestCase.class,
			VodCobraTestCase.class,
			VodDatabaseLifecycleTestCase.class,
			VodDatabaseTestCase.class,
			VodEventTestCase.class,
			VodJviTestCase.class,
			VodProviderTestCase.class,
			VodSimpleObjectContainerTestCase.class,
			ReplicationTimeoutTestCase.class,
			ReplicationViaIntermediateServerTestCase.class,
			PortSetupTestCases.class,
		};
	}

}
