/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.monitoring;

import com.db4o.config.*;
import com.db4o.internal.*;

/**
 * publishes statistics about file IO to JMX.
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class IOMonitoringSupport implements ConfigurationItem{

	public void apply(InternalObjectContainer container) {
		
	}

	public void prepare(Configuration configuration) {
		configuration.storage(new MonitoredStorage(configuration.storage()));
	}

}
