/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.events;

import com.db4o.events.*;
import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class EventArgsTransactionTestCase extends AbstractDb4oTestCase {
	
	public static class Item {
	}

	public void testTransactionInEventArgs() {
		EventRegistry factory = EventRegistryFactory.forObjectContainer(db());
		final BooleanByRef called = new BooleanByRef();
		final ObjectByRef foundTrans = new ObjectByRef();
		factory.creating().addListener(new EventListener4() {
			public void onEvent(Event4 e, EventArgs args) {
				called.value = true;
				foundTrans.value = ((TransactionalEventArgs)args).transaction();
			}
		});
		db().store(new Item());
		db().commit();
		Assert.isTrue(called.value);
		Assert.areSame(trans(), foundTrans.value);
	}
	
	public static void main(String[] args) {
		new EventArgsTransactionTestCase().runAll();
	}

}
