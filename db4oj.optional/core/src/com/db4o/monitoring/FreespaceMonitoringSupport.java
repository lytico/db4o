/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.monitoring;

import com.db4o.config.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.freespace.*;
import com.db4o.internal.slots.*;

/**
 * Publishes statistics about freespace to JMX.
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class FreespaceMonitoringSupport implements ConfigurationItem {

	public void apply(InternalObjectContainer container) {
		if(! (container instanceof LocalObjectContainer) || container.configImpl().isReadOnly()){
			return;
		}
		LocalObjectContainer localObjectContainer = (LocalObjectContainer) container;
		FreespaceManager freespaceManager = localObjectContainer.freespaceManager();
		final Freespace freespace = Db4oMBeans.newFreespaceMBean(container);
		freespaceManager.listener(freespace);
		freespaceManager.traverse(new Visitor4<Slot>() {
			public void visit(Slot slot) {
				freespace.slotAdded(slot.length());
			}
		});
	}
	
	public void prepare(Configuration configuration) {
		// do nothing
	}



}
