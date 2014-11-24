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
public class ListTypeHandlerStringElementTestSuite extends FixtureBasedTestSuite implements Db4oTestCase {
	
	
	public FixtureProvider[] fixtureProviders() {
		ListTypeHandlerTestElementsSpec[] elementSpecs = {
				ListTypeHandlerTestVariables.STRING_ELEMENTS_SPEC,
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
			ListTypeHandlerStringElementTestUnit.class,
		};
	}

	public static class ListTypeHandlerStringElementTestUnit extends ListTypeHandlerTestUnitBase {
		
		public void testSuccessfulEndsWithQuery() throws Exception {
	    	Query q = newQuery(itemFactory().itemClass());
	    	q.descend(AbstractItemFactory.LIST_FIELD_NAME).constrain(successfulEndChar()).endsWith(false);
	    	assertQueryResult(q, true);
		}
		
		public void testFailingEndsWithQuery() throws Exception {
	    	Query q = newQuery(itemFactory().itemClass());
	    	q.descend(AbstractItemFactory.LIST_FIELD_NAME).constrain(failingEndChar()).endsWith(false);
	    	assertQueryResult(q, false);
		}

		private String successfulEndChar() {
			return String.valueOf(endChar());
		}

		private String failingEndChar() {
			return String.valueOf(endChar() + 1);
		}

		private char endChar() {
			String str = (String)elements()[0];
			return str.charAt(str.length()-1);
		}
	}

}
