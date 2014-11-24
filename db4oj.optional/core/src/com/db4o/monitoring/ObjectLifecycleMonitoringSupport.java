/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.monitoring;

import com.db4o.config.*;
import com.db4o.events.*;
import com.db4o.internal.*;

/**
 * Publishes statistics about object lifecycle events to JMX.
 * 
 * In client/server setups the counters ObjectsStoredPerSec,
 * ObjectsActivatedPerSec and ObjectsDeactivatedPerSec are
 * only tracked on the client side. The counter 
 * ObjectsDeletedPerSec is only tracked on the server side.
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ObjectLifecycleMonitoringSupport implements ConfigurationItem {

	public void apply(InternalObjectContainer container) {
		
		final ObjectLifecycle objectLifecycle = Db4oMBeans.newObjectLifecycleMBean(container);
		
		EventRegistry eventRegistry = EventRegistryFactory.forObjectContainer(container);
		
		eventRegistry.created().addListener(new EventListener4<ObjectInfoEventArgs>() {
			public void onEvent(Event4<ObjectInfoEventArgs> e, ObjectInfoEventArgs args) {
				objectLifecycle.notifyStored();
			}
		});
		
		eventRegistry.updated().addListener(new EventListener4<ObjectInfoEventArgs>() {
			public void onEvent(Event4<ObjectInfoEventArgs> e, ObjectInfoEventArgs args) {
				objectLifecycle.notifyStored();
			}
		});
		
		eventRegistry.activated().addListener(new EventListener4<ObjectInfoEventArgs>() {
			public void onEvent(Event4<ObjectInfoEventArgs> e, ObjectInfoEventArgs args) {
				objectLifecycle.notifyActivated();
			}
		});
		
		eventRegistry.deactivated().addListener(new EventListener4<ObjectInfoEventArgs>() {
			public void onEvent(Event4<ObjectInfoEventArgs> e, ObjectInfoEventArgs args) {
				objectLifecycle.notifyDeactivated();
			}
		});
		
		if(container.isClient()){
			return;
		}
		
		eventRegistry.deleted().addListener(new EventListener4<ObjectInfoEventArgs>() {
			public void onEvent(Event4<ObjectInfoEventArgs> e, ObjectInfoEventArgs args) {
				objectLifecycle.notifyDeleted();
			}
		});
	

		
	}

	public void prepare(Configuration configuration) {
		// TODO Auto-generated method stub
		
	}

}
