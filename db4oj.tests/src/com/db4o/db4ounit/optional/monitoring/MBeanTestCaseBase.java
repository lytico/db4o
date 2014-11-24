/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.optional.monitoring;

import java.util.*;

import javax.management.*;

import com.db4o.config.*;
import com.db4o.internal.config.*;
import com.db4o.monitoring.*;

import db4ounit.*;
import db4ounit.extensions.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public abstract class MBeanTestCaseBase extends AbstractDb4oTestCase {

	public static class Item {
		
		public Item(String id) {
			_id = id;
		}

		public String _id;
	}
	
	@Override
	protected void configure(Configuration legacy) throws Exception {
		CommonConfiguration config = Db4oLegacyConfigurationBridge.asCommonConfiguration(legacy);
		config.environment().add(_clock);
	}

	protected void exercisePerSecondCounter(final String beanAttributeName, final Runnable counterIncrementTrigger) {
		Assert.areEqual(0.0, bean().getAttribute(beanAttributeName));
		
		for (int i=0; i<3; ++i) {
			counterIncrementTrigger.run();
			counterIncrementTrigger.run();
			advanceClock(1000);
			Assert.areEqual(2.0, bean().getAttribute(beanAttributeName));
		}
	}

	protected List<Notification> startCapturingNotifications(final String notificationType) throws JMException {
		final List<Notification> notifications = new ArrayList<Notification>();
		
		bean().addNotificationListener(new NotificationListener() {
			public void handleNotification(Notification notification, Object handback) {
				notifications.add(notification);
			}
		}, new NotificationFilter() {
			
			public boolean isNotificationEnabled(Notification notification) {
				return notificationType.equals(notification.getType());
			}
		});
		
		return notifications;
	}

	protected abstract Class<?> beanInterface();
	protected abstract String beanID();
	
	protected MBeanProxy bean() {
		if (_bean == null) {
			_bean = new MBeanProxy(Db4oMBeans.mBeanNameFor(beanInterface(), beanID()));
		}
		return _bean;
	}
	
	protected MBeanProxy fileSessionBean() {
		if (_fileSessionBean == null) {
			_fileSessionBean = new MBeanProxy(Db4oMBeans.mBeanNameFor(beanInterface(), Db4oMBeans.mBeanIDForContainer(fileSession())));
		}
		return _fileSessionBean;
	}
	
	protected void advanceClock(int time) {
		_clock.advance(time);
	}
	
	protected final transient ClockMock _clock = new ClockMock();
	
	protected transient MBeanProxy _bean;
	
	protected transient MBeanProxy _fileSessionBean;
	
}
