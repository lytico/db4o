/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import db4ounit.extensions.*;

public class AllTests extends Db4oConcurrencyTestSuite {
	
	public static void main(String[] args) {
		new AllTests().runConcurrencyAll();
	}

	protected Class[] testCases() {
		return new Class[] { 
				ArrayNOrderTestCase.class, 
				ByteArrayTestCase.class,
				CascadeDeleteDeletedTestCase.class,
				CascadeDeleteFalseTestCase.class,
				CascadeOnActivateTestCase.class,
				CascadeOnUpdateTestCase.class,
				CascadeOnUpdate2TestCase.class,
				CascadeToVectorTestCase.class,
				CaseInsensitiveTestCase.class,
				Circular1TestCase.class,
				ClientDisconnectTestCase.class,
				CreateIndexInheritedTestCase.class,
				DeepSetTestCase.class,
				DeleteDeepTestCase.class,
				DifferentAccessPathsTestCase.class,
				ExtMethodsTestCase.class,
				GetAllTestCase.class,
				GreaterOrEqualTestCase.class,
				IndexedByIdentityTestCase.class,
				IndexedUpdatesWithNullTestCase.class,
				InternStringsTestCase.class,
				InvalidUUIDTestCase.class,
				IsStoredTestCase.class,
				MessagingTestCase.class,
				MultiDeleteTestCase.class,
				MultiLevelIndexTestCase.class,
				NestedArraysTestCase.class,
				ObjectSetIDsTestCase.class,
				ParameterizedEvaluationTestCase.class,
				PeekPersistedTestCase.class,
				PersistStaticFieldValuesTestCase.class,
				QueryForUnknownFieldTestCase.class,
				QueryNonExistantTestCase.class,
				ReadObjectNQTestCase.class,
				ReadObjectQBETestCase.class,
				ReadObjectSODATestCase.class,
				RefreshTestCase.class,
				UpdateObjectTestCase.class,
		};
	}

}
