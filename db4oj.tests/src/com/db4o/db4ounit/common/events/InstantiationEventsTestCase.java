/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.events;

import com.db4o.config.*;
import com.db4o.events.*;
import com.db4o.internal.*;

import db4ounit.*;

public class InstantiationEventsTestCase extends EventsTestCaseBase {

	protected void configure(Configuration config) {
		config.activationDepth(0);
	}
	
	public void testInstantiationEvents() {
		
		final EventLog instantiatedLog = new EventLog();
		
		eventRegistry().instantiated().addListener(new EventListener4<ObjectInfoEventArgs>() {
			public void onEvent(Event4 e, ObjectInfoEventArgs args) {
				assertClientTransaction(args);
				
				instantiatedLog.xed = true;
				Object obj = args.object();
				final ObjectReference objectReference = trans().referenceSystem().referenceForObject(obj);
				
				Assert.isNotNull(objectReference);
				Assert.areSame(objectReference, args.info());
			}
		});
		
		retrieveOnlyInstance(Item.class);
		
		Assert.isFalse(instantiatedLog.xing);
		Assert.isTrue(instantiatedLog.xed);
	}
}
