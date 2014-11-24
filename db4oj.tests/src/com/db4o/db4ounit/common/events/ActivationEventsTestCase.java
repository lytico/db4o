/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.events;

import com.db4o.config.*;
import com.db4o.events.*;

import db4ounit.*;

public class ActivationEventsTestCase extends EventsTestCaseBase {
	
	protected void configure(Configuration config) {
		config.activationDepth(1);
	}
	
	public void testActivationEvents() {
		
		final EventLog activationLog = new EventLog();
		
		eventRegistry().activating().addListener(new EventListener4<CancellableObjectEventArgs>() {
			public void onEvent(Event4 e, CancellableObjectEventArgs args) {
				assertClientTransaction(args);
				activationLog.xing = true;
			}
		});
		eventRegistry().activated().addListener(new EventListener4<ObjectInfoEventArgs>() {
			public void onEvent(Event4 e, ObjectInfoEventArgs args) {
				assertClientTransaction(args);
				activationLog.xed = true;
			}
		});
		
		retrieveOnlyInstance(Item.class);
		
		Assert.isTrue(activationLog.xing);
		Assert.isTrue(activationLog.xed);
	}
}
