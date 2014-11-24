/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.internal;

import db4ounit.extensions.*;


public class AllTests extends Db4oTestSuite {
    
    public static void main(String[] args) {
        new AllTests().runSolo();
    }
	
	protected Class[] testCases() {
		return new Class[] {
				
			com.db4o.db4ounit.common.internal.convert.AllTests.class,
			com.db4o.db4ounit.common.internal.metadata.AllTests.class,
			com.db4o.db4ounit.common.internal.query.AllTests.class,
			
			BlockConverterTestCase.class,
            ClassMetadataTestCase.class,
			ClassMetadataTypeHandlerIntegrationTestCase.class,
			Comparable4TestCase.class,
			DeactivateTestCase.class,
		    EmbeddedClientObjectContainerTestCase.class,
		    EventDispatchersTestCase.class,
		    InternalObjectContainerAPITestCase.class,
		    MarshallerFamilyTestCase.class,
		    MarshallingBufferTestCase.class,
		    MarshallingContextTestCase.class,
		    Platform4TestCase.class,
			SerializerTestCase.class,
			TransactionLocalTestCase.class,
			TransactionTestCase.class,
		};
	}

}
