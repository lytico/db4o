/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.staging;

import com.db4o.db4ounit.common.defragment.*;

import db4ounit.extensions.ComposibleTestSuite;

public class AllTests extends ComposibleTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
		return composeTests(
					new Class[] {			
							/**
							 *  When you add a test here, make sure you create a Jira issue. 
							 */
							ActivateDepthTestCase.class,
							InterfaceQueryTestCase.class, // COR-1131
							GenericClassWithExistingSuperClassTestCase.class, // COR-1959
							LazyQueryDeleteTestCase.class,
							OldVersionReflectFieldAfterRefactorTestCase.class, // COR-1937
							StoredClassUnknownClassQueryTestCase.class, // COR-1542
							UnavailableEnumTestCase.class, // special case of COR-1959
							UntypedFieldSortingTestCase.class,
					});
	}
	
	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	@Override
	protected Class[] composeWith() {
		return new Class[] {
						ClientServerPingTestCase.class,
						DeepPrefetchingCacheConcurrencyTestCase.class, // COR-1762
						OwnCommitCallbackFlaggedEmbeddedTestSuite.class, // COR-1964
						PingTestCase.class,
						TAUnavailableClassAtServer.class, //COR-1987
					};
	}
}
