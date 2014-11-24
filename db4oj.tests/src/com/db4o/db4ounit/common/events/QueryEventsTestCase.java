/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.events;

import com.db4o.events.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class QueryEventsTestCase extends AbstractDb4oTestCase {
	
	public static class Item {
		public int id;
	}

	private boolean queryStarted;
	private boolean queryFinished;
	
	@Override
	protected void db4oSetupAfterStore() throws Exception {
		EventRegistry events = eventRegistry();
		
		events.queryStarted().addListener(new EventListener4<QueryEventArgs>() {
			public void onEvent(Event4<QueryEventArgs> e, QueryEventArgs args) {
				queryStarted = true;
			}
		});
		
		events.queryFinished().addListener(new EventListener4<QueryEventArgs>() {
			
			public void onEvent(Event4<QueryEventArgs> e, QueryEventArgs args) {
				queryFinished = true;				
			}
		});
	}
	
	public void testSodaQueryLifeCycleEvents() {
		Query query = newQuery(Item.class);
		query.descend("id").constrain(42);
		
		query.execute();
		
		assertEventsRaised();
	}
	
	public void testClassOnlyQueryLifeCycleEvents() {
		assertClassOnlyQuery(Item.class);		
	}
	
	public void testUntypedClassOnlyQueryLifeCycleEvents() {
		assertClassOnlyQuery(Object.class);
	}

	private <T> void assertClassOnlyQuery(Class<T> clazz) {
		Query query = newQuery(clazz);
		
		query.execute();		
		assertEventsRaised();
	}
	
	private void assertEventsRaised() {
		Assert.isTrue(queryStarted);
		Assert.isTrue(queryFinished);
	}
}
