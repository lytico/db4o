package com.db4o.db4ounit.jre5.collections.typehandler;

import java.util.*;

import com.db4o.query.*;
import com.db4o.typehandlers.*;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;
import db4ounit.fixtures.*;

/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class MapTypeHandlerTestSuite extends FixtureBasedTestSuite implements Db4oTestCase  {

	@Override
	public FixtureProvider[] fixtureProviders() {
		return new FixtureProvider[]{
				new Db4oFixtureProvider(),
				MapTypeHandlerTestVariables.MAP_FIXTURE_PROVIDER,
				MapTypeHandlerTestVariables.MAP_KEYS_PROVIDER,
				MapTypeHandlerTestVariables.MAP_VALUES_PROVIDER,
				MapTypeHandlerTestVariables.TYPEHANDLER_FIXTURE_PROVIDER,
		};
	}

	@Override
	public Class[] testUnits() {
		return new Class[]{
			MapTypeHandlerUnitTestCase.class,
		};
	}
	
	public static class MapTypeHandlerUnitTestCase extends CollectionTypeHandlerUnitTest {
		
		protected void fillItem(Object item) {
			fillMapItem(item);
		}

		protected void assertContent(Object item) {
			assertMapContent(item);
		}

		protected void assertPlainContent(Object item) {
			assertPlainMapContent((Map) item);
		}

		protected AbstractItemFactory itemFactory() {
			return (AbstractItemFactory) MapTypeHandlerTestVariables.MAP_IMPLEMENTATION.value();
		}
		
		protected TypeHandler4 typeHandler() {
		    return (TypeHandler4) MapTypeHandlerTestVariables.MAP_TYPEHANDER.value();
		}
		
		protected ListTypeHandlerTestElementsSpec elementsSpec() {
			return (ListTypeHandlerTestElementsSpec) MapTypeHandlerTestVariables.MAP_KEYS_SPEC.value();
		}

		protected void assertCompareItems(Object element, boolean successful) {
			Query q = newQuery();
	    	Object item = itemFactory().newItem();
	    	Map map = mapFromItem(item);
			map.put(element, values()[0]);
	    	q.constrain(item);
			assertQueryResult(q, successful);
		}    
		
	}

}
