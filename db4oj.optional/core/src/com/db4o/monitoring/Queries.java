/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.monitoring;

import javax.management.*;

import com.db4o.*;
import com.db4o.diagnostic.*;
import com.db4o.monitoring.internal.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
class Queries extends NotificationEmitterMBean implements QueriesMBean {

	private final TimedReading _classIndexScans = TimedReading.newPerSecond();
	private final TimedReading _queries = TimedReading.newPerSecond();
	private final AveragingTimedReading _queryExecutionTime = new AveragingTimedReading();

	public Queries(ObjectContainer db, Class<?> type) throws JMException {
		super(db, type);
	}

	private static String classIndexScanNotificationType() {
		return LoadedFromClassIndex.class.getName();
	}
	
	public double getClassIndexScansPerSecond() {
		return _classIndexScans.read();
	}

	public double getAverageQueryExecutionTime() {
		return _queryExecutionTime.read();
	}

	public double getQueriesPerSecond() {
		return _queries.read();
	}

	public MBeanNotificationInfo[] getNotificationInfo() {
		return new MBeanNotificationInfo[] {
			new MBeanNotificationInfo(
					new String[] { classIndexScanNotificationType() },
					Notification.class.getName(),
					"Notification about class index scans."),
			
		};
	}
	
	public void notifyClassIndexScan(LoadedFromClassIndex d) {
		
		_classIndexScans.increment();
		
		sendNotification(classIndexScanNotificationType(), d.problem(), d.reason());
	}

	public void notifyQueryStarted() {
		_queries.increment();
		
		_queryExecutionTime.eventStarted();
	}	
	
	public void notifyQueryFinished() {
		
		_queryExecutionTime.eventFinished();
	}
}
