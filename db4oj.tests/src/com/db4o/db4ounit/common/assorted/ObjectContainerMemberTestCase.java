/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.*;
import com.db4o.events.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ObjectContainerMemberTestCase extends AbstractDb4oTestCase {
	
	public static class Item {
		
		public ObjectContainer _typedObjectContainer;
		
		public Object _untypedObjectContainer;

	}

	public void test() throws Exception{
		EventRegistry eventRegistryFactory = EventRegistryFactory.forObjectContainer(db());
		eventRegistryFactory.creating().addListener(new EventListener4<CancellableObjectEventArgs>(){
			public void onEvent(Event4<CancellableObjectEventArgs> e,
					CancellableObjectEventArgs args) {
				Object obj = args.object();
				Assert.isFalse(obj instanceof ObjectContainer);
			}
		});
		Item item = new Item();
		item._typedObjectContainer = db();
		item._untypedObjectContainer = db();
		store(item);

		// Special case: Cascades activation to existing ObjectContainer member
		db().queryByExample(Item.class).next();
		
	}

}
