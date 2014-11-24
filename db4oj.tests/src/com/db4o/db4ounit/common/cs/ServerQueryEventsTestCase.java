/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.cs;

import java.util.*;

import com.db4o.events.*;
import com.db4o.foundation.*;
import com.db4o.query.*;

import db4ounit.*;

public class ServerQueryEventsTestCase extends ClientServerTestCaseBase {
	
	public void testConstrainedQuery() {
		
		assertQueryEvents(new Runnable() { public void run() {
			final Query query = newQuery(Item.class);
			query.descend("id").constrain(42);
			query.execute();
		}});
	}

	public void testClassOnlyQuery() {
		assertQueryEvents(new Runnable() { public void run() {
			newQuery(Item.class).execute();
		}});
		
	}
	
	public void testGetAllQuery() {
		assertQueryEvents(new Runnable() { public void run() {
			newQuery().execute();
		}});
	}
	
	private void assertQueryEvents(final Runnable query) {
		final ArrayList<String> events = new ArrayList<String>();
		
		final EventRegistry eventRegistry = EventRegistryFactory.forObjectContainer(fileSession());
		eventRegistry.queryStarted().addListener(new EventListener4<QueryEventArgs>() {
			public void onEvent(Event4<QueryEventArgs> e, QueryEventArgs args) {
				events.add(QUERY_STARTED);
			}
		});
		eventRegistry.queryFinished().addListener(new EventListener4<QueryEventArgs>() {
			public void onEvent(Event4<QueryEventArgs> e, QueryEventArgs args) {
				events.add(QUERY_FINISHED);
			}
		});
		
		query.run();
		
		final String[] expected = new String[] { QUERY_STARTED, QUERY_FINISHED };
		Iterator4Assert.areEqual(expected, Iterators.iterator(events));
	}
	
	private static final String QUERY_FINISHED = "query finished";
	private static final String QUERY_STARTED = "query started";
	
	public static final class Item {
		public int id;
	}

}
