/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre5.collections.typehandler;

import java.util.*;

import com.db4o.query.*;
import com.db4o.typehandlers.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;
import db4ounit.fixtures.*;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
@SuppressWarnings("unchecked")
public class ListTypeHandlerTestSuite extends FixtureBasedTestSuite implements Db4oTestCase {
	
	
	public FixtureProvider[] fixtureProviders() {
		ListTypeHandlerTestElementsSpec[] elementSpecs = {
				ListTypeHandlerTestVariables.STRING_ELEMENTS_SPEC,
				ListTypeHandlerTestVariables.INT_ELEMENTS_SPEC,
				ListTypeHandlerTestVariables.OBJECT_ELEMENTS_SPEC,
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
			ListTypeHandlerTestUnit.class,
		};
	}

	public static class ListTypeHandlerTestUnit extends CollectionTypeHandlerUnitTest {
		
		protected AbstractItemFactory itemFactory() {
			return (AbstractItemFactory) ListTypeHandlerTestVariables.LIST_IMPLEMENTATION.value();
		}
		
		protected TypeHandler4 typeHandler() {
		    return (TypeHandler4) ListTypeHandlerTestVariables.LIST_TYPEHANDER.value();
		}

		protected ListTypeHandlerTestElementsSpec elementsSpec() {
			return (ListTypeHandlerTestElementsSpec) ListTypeHandlerTestVariables.ELEMENTS_SPEC.value();
		}    

		protected void fillItem(Object item) {
			fillListItem(item);
		}
		
		protected void assertContent(Object item) {
			assertListContent(item);
		}

		protected void assertPlainContent(Object item) {
			assertPlainListContent((List) item);
		}
		
	    protected void assertCompareItems(Object element, boolean successful) {
			Query q = newQuery();
	    	Object item = itemFactory().newItem();
	    	List list = listFromItem(item);
			list.add(element);
	    	q.constrain(item);
			assertQueryResult(q, successful);
		}
	    
	    public void testActivation(){
	        Object item = retrieveItemInstance();
	        List list = listFromItem(item);
	        Assert.areEqual(expectedElementCount(), list.size());
	        Object element = list.get(0);
	        if(db().isActive(element)){
	            db().deactivate(item, Integer.MAX_VALUE);
	            Assert.isFalse(db().isActive(element));
	            db().activate(item, Integer.MAX_VALUE);
	            Assert.isTrue(db().isActive(element));
	        }
	    }
		
	}

}
