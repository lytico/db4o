/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.events;

import com.db4o.events.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class EventsTestCaseBase extends AbstractDb4oTestCase {

	public static final class Item {
		public int id;
		
		public Item() {
		}
		
		public Item(int id) {
			this.id = id;
		}
	}
	
	protected static final class EventLog {
		public boolean xing;
		public boolean xed;
	}
	
	protected void store() throws Exception {
		store(new Item(1));
	}

	protected void assertClientTransaction(EventArgs args) {
        Assert.areSame(trans(), ((TransactionalEventArgs)args).transaction());
    }
}