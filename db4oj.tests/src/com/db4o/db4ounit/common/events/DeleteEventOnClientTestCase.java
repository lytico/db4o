/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */
package com.db4o.db4ounit.common.events;

import com.db4o.events.*;

import db4ounit.*;
import db4ounit.extensions.fixtures.*;

public class DeleteEventOnClientTestCase extends EventsTestCaseBase implements OptOutSolo {
	public static void main(String[] args) {
		new DeleteEventOnClientTestCase().runAll();
	}
	
	public void testAttachingToDeletingEventThrows() {
		if (isEmbedded()) return;
		
		Assert.expect(IllegalArgumentException.class, new CodeBlock() {
			public void run() throws Throwable {
				eventRegistry().deleting().addListener(new EventListener4(){
					public void onEvent(Event4 e, EventArgs args) {
					}
				});
			}
		});			
	}
	
	public void testAttachingToDeleteEventThrows() {
			if (isEmbedded()) return;
				
			Assert.expect(IllegalArgumentException.class, new CodeBlock() {
				public void run() throws Throwable {
					eventRegistry().deleted().addListener(new EventListener4(){
						public void onEvent(Event4 e, EventArgs args) {
						}
					});
				}
			});			
	}
}
