/* Copyright (C) 2011 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.events;

import com.db4o.events.*;
import com.db4o.foundation.*;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class CallbackTrackingTestCase extends AbstractDb4oTestCase implements OptOutAllButNetworkingCS {

	public void testStaticFields() throws Exception {
		
		assertQueryOnCallBack(ItemWithStaticField.class);
	}
	
	/** @sharpen.ignore */
	public void testEnum() throws Exception {
		
		assertQueryOnCallBack(ItemWithEnum.class);
	}

	private void assertQueryOnCallBack(final Class classConstraint) throws InterruptedException {
		final Lock4 lock = new Lock4();
		
		db().store(new Item(42));
		
		final EventRegistry eventRegistry = eventRegistry();
		eventRegistry.committed().addListener(new EventListener4<CommitEventArgs>() {
			
			@Override
			public void onEvent(Event4<CommitEventArgs> e, CommitEventArgs args) {

				try {					
					args.objectContainer().query(classConstraint);
				}
				finally {
					
					lock.run(new Closure4() {
						
						@Override
						public Object run() {
							lock.awake();
							return null;
						}
						
					});
				}
			}
		});
		
		synchronized (lock) {
			db().commit();
			lock.snooze(5000);	
		}
		
	}
		
	@SuppressWarnings("unused")
	private static class ItemWithStaticField {
		public static int i;		
	}
	
	@SuppressWarnings("unused")
	private static class Item {
		
		public int id;

		public Item(int id) {
			this.id = id;
		}		
	}
	
	/** @sharpen.ignore */
	private static class ItemWithEnum extends Item {
		public ItemEnum e;
		
		public ItemWithEnum(int id, ItemEnum e) {
			super(id);
			this.e = e;
		}
	}

	/** @sharpen.ignore */
	private static enum ItemEnum {
		one,
		two
	}
	
}
