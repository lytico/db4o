/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.internal.monitoring;

import java.util.*;

import javax.management.*;

import com.db4o.*;
import com.db4o.events.*;
import com.db4o.monitoring.*;
import static com.db4o.foundation.Environments.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class Db4oMBeanRegistryImpl implements Db4oMBeanRegistry {

	private Set<Db4oMBean> _beans = new HashSet<Db4oMBean>();
	
	public Db4oMBeanRegistryImpl() {
		ObjectContainer db = my(ObjectContainer.class);
		EventRegistry eventRegistry = EventRegistryFactory.forObjectContainer(db);
		eventRegistry.opened().addListener(new EventListener4<ObjectContainerEventArgs>() {
			public void onEvent(Event4<ObjectContainerEventArgs> event, ObjectContainerEventArgs args) {
				register();
			}
		});
		eventRegistry.closing().addListener(new EventListener4<ObjectContainerEventArgs>() {
			public void onEvent(Event4<ObjectContainerEventArgs> event, ObjectContainerEventArgs args) {
				unregister();
			}
		});
	}
	
	public void add(Db4oMBean bean) {
		_beans.add(bean);
	}

	public void register() {
		for (Db4oMBean bean : _beans) {
			try {
				bean.register();
			} 
			catch (JMException exc) {
				exc.printStackTrace();
			}
		}
	}

	public void unregister() {
		for (Db4oMBean bean : _beans) {
			try {
				bean.unregister();
			} 
			catch (JMException exc) {
				exc.printStackTrace();
			}
		}
	}

}
