/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.monitoring;

import javax.management.*;

import com.db4o.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public abstract class NotificationEmitterMBean extends MBeanRegistrationSupport implements NotificationEmitter{

	private final NotificationBroadcasterSupport _notificationSupport = new NotificationBroadcasterSupport();
	
	public NotificationEmitterMBean(ObjectContainer db, Class<?> type) throws JMException {
		super(db, type);
	}

	public void removeNotificationListener(NotificationListener listener,
			NotificationFilter filter, Object handback)
			throws ListenerNotFoundException {
		_notificationSupport.removeNotificationListener(listener, filter, handback);
	}

	public void addNotificationListener(NotificationListener listener,
			NotificationFilter filter, Object handback)
			throws IllegalArgumentException {
		_notificationSupport.addNotificationListener(listener, filter, handback);
	}
	
	public void removeNotificationListener(NotificationListener listener)
			throws ListenerNotFoundException {
		_notificationSupport.removeNotificationListener(listener);
	}


	protected void sendNotification(final String notificationType,
			final String message, final Object userData) {
		final Notification notification = new Notification(notificationType, objectName(), 0, message);
		notification.setUserData(userData);
		_notificationSupport.sendNotification(notification);
	}


}
