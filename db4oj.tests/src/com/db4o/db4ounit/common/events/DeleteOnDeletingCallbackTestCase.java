/* Copyright (C) 2007 - 2010 Versant Inc. http://www.db4o.com */

/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.common.events;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.cs.internal.*;
import com.db4o.events.*;
import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class DeleteOnDeletingCallbackTestCase extends AbstractDb4oTestCase {
	
	public static class Item {
	}
	
	public static class RootItem {
		
		public Item child;
		
		public RootItem() {
		}
		
		public void objectOnDelete(ObjectContainer container) {
			container.delete(child);
		}
	}
	
	@Override
	protected void store() throws Exception {
		store(new RootItem());
	}
	
	public void test() throws Exception {
		
		final BooleanByRef disconnected = new BooleanByRef();
		final Lock4 lock = new Lock4();
		if(isNetworking()){
			Db4oClientServerFixture clientServerFixture = (Db4oClientServerFixture) fixture();
			ObjectServerEvents objectServerEvents = (ObjectServerEvents) clientServerFixture.server();
			objectServerEvents.clientDisconnected().addListener(new EventListener4<StringEventArgs>() { public void onEvent(Event4<StringEventArgs> e, StringEventArgs args) {
				lock.run(new Closure4() { public Object run() {
					disconnected.value = true;
					lock.awake();
					return null;
				}});
				
			}});
		}
		
		final RootItem root = retrieveOnlyInstance(RootItem.class);
		root.child = new Item();
		db().store(root);
		db().delete(root);
		reopen();
		
		if(isNetworking()){
			lock.run(new Closure4() {
				public Object run() {
					if(! disconnected.value){
						lock.snooze(1000000);
					}
					return null;
				}
			});
		}
		
		assertClassIndexIsEmpty();
	}

	private void assertClassIndexIsEmpty() {
	    Iterator4Assert.areEqual(new Object[0], getAllIds());
    }

	private IntIterator4 getAllIds() {
	    return fileSession().getAll(fileSession().transaction(), QueryEvaluationMode.IMMEDIATE).iterateIDs();
    }

}
