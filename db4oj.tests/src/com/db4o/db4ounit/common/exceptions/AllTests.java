/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.exceptions;

import db4ounit.extensions.*;

public class AllTests extends ComposibleTestSuite {

	protected Class[] testCases() {
		return composeTests(new Class[] { 
								ActivationExceptionBubblesUpTestCase.class,
								BackupCSExceptionTestCase.class,
								CompositeDb4oExceptionTestCase.class,
								DatabaseClosedExceptionTestCase.class,
								DatabaseReadonlyExceptionTestCase.class,
								GlobalOnlyConfigExceptionTestCase.class,
								ObjectCanActiviateExceptionTestCase.class,
								ObjectCanDeleteExceptionTestCase.class,
								ObjectOnDeleteExceptionTestCase.class,
								ObjectCanNewExceptionTestCase.class,
								StoreExceptionBubblesUpTestCase.class, 
								StoredClassExceptionBubblesUpTestCase.class,
								TSerializableOnInstantiateCNFExceptionTestCase.class,
								TSerializableOnInstantiateIOExceptionTestCase.class,
								TSerializableOnStoreExceptionTestCase.class,
							});
	}
	
	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	@Override
	protected Class[] composeWith() {
		return new Class[] { 
					BackupDb4oIOExceptionTestCase.class,
					IncompatibleFileFormatExceptionTestCase.class, 
					InvalidSlotExceptionTestCase.class,
					OldFormatExceptionTestCase.class,
					com.db4o.db4ounit.common.exceptions.propagation.AllTests.class,
					InvalidPasswordTestCase.class
				};
	}
}
