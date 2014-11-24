/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.optional.monitoring;

import java.util.*;

import javax.management.*;

import com.db4o.config.*;
import com.db4o.diagnostic.*;
import com.db4o.foundation.*;
import com.db4o.monitoring.*;
import com.db4o.query.Query;

import db4ounit.*;
import db4ounit.extensions.OptOutNotSupportedJavaxManagement;
import db4ounit.extensions.fixtures.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class QueryMonitoringSupportTestCase extends QueryMonitoringTestCaseBase implements CustomClientServerConfiguration, OptOutNotSupportedJavaxManagement {
	
	@Override
	protected void configure(Configuration config) throws Exception {
		super.configure(config);
		config.add(new QueryMonitoringSupport());
	}

	public void configureClient(Configuration config) throws Exception {
	}

	public void configureServer(Configuration config) throws Exception {
		configure(config);
	}
	
	@Override
	protected Class<?> beanInterface() {
		return QueriesMBean.class;
	}
	
	@Override
	protected void store() throws Exception {
		db().store(new Item("foo"));
	}
	
	public void testClassIndexScan() throws Exception {
		exercisePerSecondCounter("ClassIndexScansPerSecond", new Runnable() { public void run() {
			triggerClassIndexScan();
		}});
	}
	
	public void testAverageQueryExecutionTime() {
		
		Assert.areEqual(0.0, bean().getAttribute("AverageQueryExecutionTime"));
		
		triggerQueryExecutionTime(1000);
		Assert.areEqual(1000.0, bean().getAttribute("AverageQueryExecutionTime"));
		
		triggerQueryExecutionTime(200);
		triggerQueryExecutionTime(500);
		Assert.areEqual(350.0, bean().getAttribute("AverageQueryExecutionTime"));
		
	}

	private void triggerQueryExecutionTime(final int executionTime) {
		final Query query = newQuery(Item.class);
		fileSession().callbacks().queryOnStarted(trans(), query);
		_clock.advance(executionTime);
		fileSession().callbacks().queryOnFinished(trans(), query);
	}
	
	public void testQueriesPerSecond() {
		
		final ByRef<Integer> queryMode = ByRef.newInstance(0);
		
		exercisePerSecondCounter("QueriesPerSecond", new Runnable() { public void run() {
			switch (queryMode.value.intValue() % 3) {
			case 0:
				triggerOptimizedQuery();
				break;
			case 1:
				triggerUnoptimizedQuery();
				break;
			case 2:
				triggerSodaQuery();
				break;
			}
			queryMode.value = queryMode.value.intValue() + 1;
		}});
	}
	
	public void testClassIndexScanNotifications() throws Exception {
		
		final List<Notification> notifications = startCapturingNotifications(classIndexScanNotificationType());
		
		triggerClassIndexScan();
		
		Assert.areEqual(1, notifications.size());
		
		final Notification notification = notifications.get(0);
		Assert.areEqual(classIndexScanNotificationType(), notification.getType());
		Assert.areEqual(Item.class.getName(), notification.getUserData());
	}

	private String classIndexScanNotificationType() {
		return LoadedFromClassIndex.class.getName();
	}

	private void triggerClassIndexScan() {
		final Query query = newQuery(Item.class);
		query.descend("_id").constrain("foo");
		query.execute().toArray();
	}
	
	private void triggerSodaQuery() {
		newQuery(Item.class).execute().toArray();
	}

	@Override
	protected String beanID() {
		return Db4oMBeans.mBeanIDForContainer(fileSession());
	}


}
