/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.monitoring;

import static com.db4o.foundation.Environments.my;

import java.lang.management.*;

import javax.management.*;

import com.db4o.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class MBeanRegistrationSupport implements Db4oMBean {

	private ObjectContainer _db;
	private ObjectName _objectName;
	private Class<?> _type;

	public MBeanRegistrationSupport(ObjectContainer db, Class<?> type) {
		_db = db;
		_type = type;
		beanRegistry().add(this);
	}

	public MBeanRegistrationSupport(ObjectName objectName) throws JMException {
		_objectName = objectName;
	}

	public void unregister() {
		if (objectName() == null) {
			return;
		}
		
		try {
			platformMBeanServer().unregisterMBean(objectName());
		} catch (JMException e) {
			e.printStackTrace();
		} finally {
			_db = null;
			_objectName = null;
		}
	}

	// FIXME
	public void register() throws JMException {
		if(platformMBeanServer().isRegistered(objectName())) {
			return;
		}
		platformMBeanServer().registerMBean(this, objectName());
	}

	private MBeanServer platformMBeanServer() {
		return ManagementFactory.getPlatformMBeanServer();
	}

	protected ObjectName objectName() {
		if(_objectName != null) {
			return _objectName;
		}
		if(_db == null) {
			return null;
		}
		_objectName = Db4oMBeans.mBeanNameFor(_type, Db4oMBeans.mBeanIDForContainer(_db));
		return _objectName;
	}

	private Db4oMBeanRegistry beanRegistry() {
		return my(Db4oMBeanRegistry.class);
	}
}
