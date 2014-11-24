/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre5.collections.typehandler;

import com.db4o.query.*;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;
import db4ounit.fixtures.*;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
@SuppressWarnings("unchecked")
public class ListTypeHandlerGreaterSmallerTestSuite extends FixtureBasedTestSuite implements Db4oTestCase {
	
	public FixtureProvider[] fixtureProviders() {
		ListTypeHandlerTestElementsSpec[] elementSpecs = {
				ListTypeHandlerTestVariables.STRING_ELEMENTS_SPEC,
				ListTypeHandlerTestVariables.INT_ELEMENTS_SPEC,
		};
		return new FixtureProvider[] {
			new Db4oFixtureProvider(),
			ListTypeHandlerTestVariables.LIST_FIXTURE_PROVIDER,
			new SimpleFixtureProvider(
				ListTypeHandlerTestVariables.ELEMENTS_SPEC,
				(Object[])elementSpecs
			),
			ListTypeHandlerTestVariables.TYPEHANDLER_FIXTURE_PROVIDER,
		};
	}

	public Class[] testUnits() { 
		return new Class[] {
			ListTypeHandlerGreaterSmallerTestUnit.class,
		};
	}

	public static class ListTypeHandlerGreaterSmallerTestUnit extends ListTypeHandlerTestUnitBase {
		
		public void testSuccessfulSmallerQuery() throws Exception {
	    	Query q = newQuery(itemFactory().itemClass());
	    	q.descend(AbstractItemFactory.LIST_FIELD_NAME).constrain(largeElement()).smaller();
	    	assertQueryResult(q, true);
		}
		
		public void testFailingGreaterQuery() throws Exception {
	    	Query q = newQuery(itemFactory().itemClass());
	    	q.descend(AbstractItemFactory.LIST_FIELD_NAME).constrain(largeElement()).greater();
	    	assertQueryResult(q, false);
		}

	}

}
