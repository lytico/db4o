/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant.eventlistener;

import static com.db4o.qlin.QLinSupport.*;

import java.util.*;

import com.db4o.*;
import com.db4o.drs.test.versant.*;
import com.db4o.drs.test.versant.data.*;
import com.db4o.drs.versant.*;
import com.db4o.drs.versant.eventprocessor.*;
import com.db4o.drs.versant.ipc.*;
import com.db4o.drs.versant.ipc.EventProcessor.EventProcessorListener;
import com.db4o.drs.versant.metadata.*;
import com.db4o.drs.versant.metadata.ObjectInfo.Operations;
import com.db4o.foundation.*;

import db4ounit.*;


public class EventListenerIntegrationTestCase extends VodProviderTestCaseBase {
	
	public void testStoreSingleObject() throws Exception {
		withEventProcessor(new Closure4<Void>() {
			public Void run() {
				final Item item = storeAndCommitItem();
				Assert.isTrue(checkObjectInfoFor(item, 3000), "Timeout: ObjectLifecycleEvent object not stored.");
				return null;
			}
		});
	}
	
	public void testStoreTwoObjects() throws Exception {
		withEventProcessor(new Closure4<Void>() {
			public Void run() {
				
				Item itemOne = new Item("one");
				Item itemTwo = new Item("two");
				_provider.storeNew(itemOne);
				_provider.storeNew(itemTwo);
				
				_provider.commit();
				
				// TODO: check we only have one commit in the event processor

				final Item item = storeAndCommitItem();
				Assert.isTrue(checkObjectInfoFor(item, 3000), "Timeout: ObjectLifecycleEvent object not stored.");
				return null;
			}
		});
	}


	public void testStartingEventProcessorTwice() throws Exception {
		for (int i = 0; i < 2; i++) {
			withEventProcessor(new Closure4<Void>() {
				public Void run() {
					storeAndCommitItem();
					return null;
				}
			});
		}
	}
	
	public void testEventProcessorReloadsClasses() throws Exception {
		for (int i = 0; i < 2; i++) {
			withEventProcessor(new Closure4<Void>() {
				public Void run() {
					storeAndCommitItem();
					return null;
				}
			});
		}
	}
	
	public void testEventProcessor10Times() throws Exception {
		for (int i = 0; i < 10; i++) {
			withEventProcessor(new Closure4<Void>() {
				public Void run() {
					storeAndCommitItem();
					return null;
				}
			});
		}
	}
	
	public void testPersistentTimestampExistsAfterEvent() throws Exception {
		withEventProcessor(new Closure4<Void>() {
			public Void run() {
				final BlockingQueue4<Long> q = new BlockingQueue<Long>();
				EventProcessorListener listener = new AbstractEventProcessorListener() {
					@Override
					public void onEvent(long loid, long version) {
						q.add(loid);
					}
					
				};
				_provider.syncEventProcessor().addListener(listener);
				storeAndCommitItem();
				q.next();
				_provider.syncEventProcessor().removeListener(listener);
				return null;
			}
		});
		Collection<CommitTimestamp> timestamps = _jdo.query(CommitTimestamp.class);
		Assert.areEqual(1, timestamps.size());
	}
	
	private Item storeAndCommitItem() {
		Item item = new Item("one");
		_provider.storeNew(item);
		_provider.commit();
		return item;
	}
	
	private Pair storeAndCommitItemByOtherUser() {
		VodJdoFacade jdo = VodJdo.createInstance(_vod);
		Item item = new Item("one");
		jdo.store(item);
		jdo.commit();
		return new Pair(jdo, item);
	}
	
	private boolean checkObjectInfoFor(final Item item,long timeout) {
		return checkObjectInfoFor(_provider, item, timeout);
	}

	private boolean checkObjectInfoFor(final LoidProvider jdo, final Item item,
			long timeout) {
		boolean result = Runtime4.retry(timeout, new Closure4<Boolean>() {
			public Boolean run() {
				
				final long objectLoid = jdo.loid(item);
				
				ObjectInfo objectInfo = prototype(ObjectInfo.class);
				ObjectSet<ObjectInfo> objectInfos = _cobra.from(ObjectInfo.class).where(objectInfo.objectLoid()).equal(objectLoid).select();
				// System.err.println("objectLifecycleEvents.size() " + objectLifecycleEvents.size());
				if(objectInfos.size() != 1){
					return false;
				}
				ObjectInfo queriedEvent = objectInfos.iterator().next();
				Assert.areEqual(Operations.CREATE.value, queriedEvent.operation());
				Assert.isGreater(1, queriedEvent.uuidLongPart());
				Assert.isGreater(1, queriedEvent.classMetadataLoid());
				return true;
			}
		} );
		return result;
	}
	
}
