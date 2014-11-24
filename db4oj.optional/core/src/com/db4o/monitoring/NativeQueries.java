/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.monitoring;

import javax.management.*;

import com.db4o.*;
import com.db4o.diagnostic.*;
import com.db4o.internal.query.*;
import com.db4o.monitoring.internal.*;
import com.db4o.query.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class NativeQueries extends NotificationEmitterMBean implements NativeQueriesMBean {
	
	private final TimedReading _nativeQueries = TimedReading.newPerSecond();
	private final TimedReading _unoptimizedNativeQueries = TimedReading.newPerSecond();

	public NativeQueries(ObjectContainer db, Class<?> type) throws JMException {
		super(db, type);
	}

	public double getUnoptimizedNativeQueriesPerSecond() {
		return _unoptimizedNativeQueries.read();
	}
	
	public double getNativeQueriesPerSecond() {
		return _nativeQueries.read();
	}
	
	public MBeanNotificationInfo[] getNotificationInfo() {
		return new MBeanNotificationInfo[] {
			new MBeanNotificationInfo(
					new String[] { unoptimizedQueryNotificationType() },
					Notification.class.getName(),
					"Notification about unoptimized native query execution."),
			
		};
	}

	private String unoptimizedQueryNotificationType() {
		return NativeQueryNotOptimized.class.getName();
	}

	public void notifyNativeQuery(NQOptimizationInfo info) {
		
		if (info.message().equals(NativeQueryHandler.UNOPTIMIZED)) {
			notifyUnoptimized(info.predicate());
		}
		
		_nativeQueries.increment();
	}
	
	private void notifyUnoptimized(Predicate predicate) {
		
		_unoptimizedNativeQueries.increment();
		sendNotification(unoptimizedQueryNotificationType(), "Unoptimized native query.", predicate.getClass().getName());
		
	}

}
