/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.events;

import java.util.*;

import com.db4o.config.*;
import com.db4o.consistency.*;
import com.db4o.events.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.slots.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

/**
 * @exclude
 */
public class ExceptionInUpdatingCallbackCorruptionTestCase extends AbstractDb4oTestCase implements OptOutMultiSession {
	
	public static void main(String[] args) {
		new ExceptionInUpdatingCallbackCorruptionTestCase().runSolo();
	}
	
	public static class Holder {
		public List<Item> list = new ArrayList<Item>();
		public Item item;
	}
	
	public static class Item {
	}
	
	private boolean doThrow;
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.objectClass(Holder.class).cascadeOnUpdate(true);
		config.objectClass(Holder.class).cascadeOnDelete(true);
	}
	
	@Override
	protected void store() throws Exception {
		store(new Holder());
		store(new Item());
	}

	@Override
	protected void db4oSetupAfterStore() throws Exception {
		EventRegistryFactory.forObjectContainer(db()).updated().addListener(new EventListener4<ObjectInfoEventArgs>() {
			@Override
			public void onEvent(Event4<ObjectInfoEventArgs> e, ObjectInfoEventArgs args) {				
				if(doThrow){
					if(args.info().getObject().getClass().equals(Item.class)){
						throw new RuntimeException();
					}
				}
			}
		});
	}

	public void testExceptionDuringItemUpdate() throws Exception{
		final Holder holder = retrieveOnlyInstance(Holder.class);
		final Item item = retrieveOnlyInstance(Item.class);
		holder.item = item;
		withException(new Block4() {
			@Override
			public void run() {
				db().store(holder, Integer.MAX_VALUE);
			}
		});
		checkConsistencyFull();		
	}

	public void testExceptionDuringExistingListUpdate() throws Exception{
		final Holder holder = retrieveOnlyInstance(Holder.class);
		Item item = retrieveOnlyInstance(Item.class);
		holder.list.add(item);
		withException(new Block4() {
			@Override
			public void run() {
				db().store(holder, Integer.MAX_VALUE);
			}
		});
		checkConsistencyFull();		
	}

	public void testExceptionDuringNewListUpdate() throws Exception{
		final Holder holder = retrieveOnlyInstance(Holder.class);
		Item item = retrieveOnlyInstance(Item.class);
		holder.list = new ArrayList<Item>();
		holder.list.add(item);
		withException(new Block4() {
			@Override
			public void run() {
				db().store(holder, Integer.MAX_VALUE);
			}
		});
		checkConsistencyFull();
	}

	private void withException(Block4 block) {
		doThrow = true;
		try {
			block.run();
		}
		catch(RuntimeException exc) {
		}
		finally {
			doThrow = false;
		}
	}
	
	private void checkConsistencyFull() throws Exception {
		checkConsistency();
		commit();
		checkConsistency();
		reopen();
		checkConsistency();
	}

	private void checkConsistency() {
		ConsistencyReport report = new ConsistencyChecker((LocalObjectContainer) container()).checkSlotConsistency();
		if(!report.consistent()) {
			Assert.fail(report.toString());
		}
	}

}

