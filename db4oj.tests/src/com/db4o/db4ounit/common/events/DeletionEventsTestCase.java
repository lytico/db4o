/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.events;

import com.db4o.config.*;
import com.db4o.events.*;

import db4ounit.*;

public class DeletionEventsTestCase extends EventsTestCaseBase {
	
	protected void configure(Configuration config) {
		config.activationDepth(1);
	}
	
	public void testDeletionEvents() {
		
		if (isEmbedded()) {
			// TODO: something wrong when embedded c/s is run as part
			// of the full test suite
			return;
		}
		final EventLog deletionLog = new EventLog();
		
		serverEventRegistry().deleting().addListener(new EventListener4() {
			public void onEvent(Event4 e, EventArgs args) {
				deletionLog.xing = true;
				assertItemIsActive(args);
			}
		});
		serverEventRegistry().deleted().addListener(new EventListener4() {
			public void onEvent(Event4 e, EventArgs args) {
				deletionLog.xed = true;
				assertItemIsActive(args);
			}
		});
		
		db().delete(retrieveOnlyInstance(Item.class));
		db().commit();
		Assert.isTrue(deletionLog.xing);
		Assert.isTrue(deletionLog.xed);
	}

	private void assertItemIsActive(EventArgs args) {
		Assert.areEqual(1, itemForEvent(args).id);
	}

	private Item itemForEvent(EventArgs args) {
		return ((Item)((ObjectEventArgs)args).object());
	}
}
