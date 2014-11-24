/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.monitoring;

import java.util.*;

import com.db4o.config.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.references.*;
import com.db4o.monitoring.internal.*;

/**
 * Publishes statistics about the ReferenceSystem to JMX.
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ReferenceSystemMonitoringSupport implements ConfigurationItem {

	private final static class MonitoringSupportReferenceSystemFactory implements ReferenceSystemFactory, DeepClone {
		
		private final HashMap<String, com.db4o.monitoring.ReferenceSystem> _mBeans;

		public MonitoringSupportReferenceSystemFactory() {
			this(new HashMap<String, ReferenceSystem>());
		}
		
		private MonitoringSupportReferenceSystemFactory(HashMap<String, ReferenceSystem> mBeans) {
			_mBeans = mBeans;
		}


		public com.db4o.internal.references.ReferenceSystem newReferenceSystem(InternalObjectContainer container) {
			return new MonitoringReferenceSystem(mBeanFor(container));
		}

		private ReferenceSystemListener mBeanFor(InternalObjectContainer container) {
			com.db4o.monitoring.ReferenceSystem mBean = _mBeans.get(container.toString());
			if(mBean == null){
				mBean = Db4oMBeans.newReferenceSystemMBean(container);
				_mBeans.put(container.toString(), mBean);
			}
			return mBean;
		}

		public Object deepClone(Object context) {
			return new MonitoringSupportReferenceSystemFactory(new HashMap<String, ReferenceSystem>(_mBeans));
		}

	}

	public void apply(InternalObjectContainer container) {
		
	}

	public void prepare(Configuration configuration) {
		((Config4Impl)configuration).referenceSystemFactory(new MonitoringSupportReferenceSystemFactory());
	}

}
