/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.events;

import com.db4o.config.*;
import com.db4o.events.*;

import db4ounit.*;
import db4ounit.extensions.fixtures.*;

public class DeletionEventExceptionTestCase extends EventsTestCaseBase implements OptOutSolo {
	
	public static void main(String[] args) {
		new DeletionEventExceptionTestCase().runAll();
	}
	
	protected void configure(Configuration config) {
		config.activationDepth(1);
	}
	
	public void testDeletionEvents() {
		serverEventRegistry().deleting().addListener(new EventListener4() {
			public void onEvent(Event4 e, EventArgs args) {
				throw new RuntimeException();
			}
		});
		final Object item = retrieveOnlyInstance(Item.class);
	    if(isEmbedded()){
	        Assert.expect( EventException.class, new CodeBlock() {
                public void run() throws Throwable {
                    db().delete(item);
                }
            });
	    }else{
	        db().delete(item);
	    }
        db().commit();
	}
}
