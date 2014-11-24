/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.events;

import com.db4o.events.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;

import db4ounit.*;

public class CreationEventsTestCase extends EventsTestCaseBase {

	public void testObjectInfoIsNotAvailableOnCreatingHandler() {
		final ByRef<Boolean> executed = ByRef.newInstance(false);
		
		eventRegistry().creating().addListener(new EventListener4<CancellableObjectEventArgs>() {
			
			public void onEvent(Event4<CancellableObjectEventArgs> e, final CancellableObjectEventArgs args) {
				Assert.expect(IllegalStateException.class, new CodeBlock() {				
					public void run() throws Throwable {
						executed.value = true;
						usefulForCSharp(args.info());						
					}

					private void usefulForCSharp(ObjectInfo info) {
						Assert.fail();
					}
				});
			}
		});
		
		store(new Item());
		Assert.isTrue(executed.value);
	}
}
