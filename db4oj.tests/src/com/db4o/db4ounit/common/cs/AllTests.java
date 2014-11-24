/* Copyright (C) 2004 - 20067 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import db4ounit.extensions.*;

public class AllTests extends Db4oTestSuite {

	public static void main(String[] args) {
		new AllTests().runAll();
    }
	
	protected Class[] testCases() {
		return new Class[] {
				
			com.db4o.db4ounit.common.cs.caching.AllTests.class,
			com.db4o.db4ounit.common.cs.config.AllTests.class,
			com.db4o.db4ounit.common.cs.objectexchange.AllTests.class,
			
			BatchActivationTestCase.class,
	        CallConstructorsConfigTestCase.class,
	        ClientDisconnectTestCase.class,
            ClientTimeOutTestCase.class,
            ClientTransactionHandleTestCase.class,
            ClientTransactionPoolTestCase.class,
            CloseServerBeforeClientTestCase.class,
            CsCascadedDeleteReaddChildReferenceTestCase.class,
            CsDeleteReaddTestCase.class,
            CsSchemaUpdateTestCase.class,
            IsAliveConcurrencyTestCase.class,
            IsAliveTestCase.class,
            NoTestConstructorsQEStringCmpTestCase.class,
            ObjectServerTestCase.class,
            PrefetchConfigurationTestCase.class,
            PrefetchIDCountTestCase.class,
            PrefetchObjectCountZeroTestCase.class,
            PrimitiveMessageTestCase.class,
            QueryConsistencyTestCase.class,
            ReferenceSystemIsolationTestCase.class,
            SendMessageToClientTestCase.class,
            ServerClosedTestCase.class,
            ServerObjectContainerIsolationTestCase.class,
            ServerPortUsedTestCase.class,
            ServerQueryEventsTestCase.class,
            ServerRevokeAccessTestCase.class,
            ServerTimeoutTestCase.class,
            ServerToClientTestCase.class,
            ServerTransactionCountTestCase.class,
            SetSemaphoreTestCase.class,
            SSLSocketTestCase.class,
            UniqueConstraintOnServerTestCase.class,
		};
	}
	
}
