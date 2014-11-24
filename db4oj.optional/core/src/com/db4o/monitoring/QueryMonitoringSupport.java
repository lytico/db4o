/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.monitoring;

import com.db4o.config.*;
import com.db4o.diagnostic.*;
import com.db4o.events.*;
import com.db4o.internal.*;
import com.db4o.internal.config.*;

/**
 * publishes statistics about Queries to JMX.
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class QueryMonitoringSupport implements ConfigurationItem {

	public void apply(InternalObjectContainer container) {
		final Queries queries = Db4oMBeans.newQueriesMBean(container);
		final CommonConfiguration config = Db4oLegacyConfigurationBridge.asCommonConfiguration(container.configure());
		config.diagnostic().addListener(new DiagnosticListener() {
			public void onDiagnostic(Diagnostic d) {
				if (d instanceof LoadedFromClassIndex) {
					queries.notifyClassIndexScan((LoadedFromClassIndex)d);
				}
			}
		});
		
		final EventRegistry events = EventRegistryFactory.forObjectContainer(container);
		events.queryStarted().addListener(new EventListener4<QueryEventArgs>() {
			public void onEvent(Event4<QueryEventArgs> e, QueryEventArgs args) {
				queries.notifyQueryStarted();
			}
		});
		
		events.queryFinished().addListener(new EventListener4<QueryEventArgs>() {
			public void onEvent(Event4<QueryEventArgs> e, QueryEventArgs args) {
				queries.notifyQueryFinished();
			}
		});
		
	}

	public void prepare(Configuration configuration) {
	}

}
