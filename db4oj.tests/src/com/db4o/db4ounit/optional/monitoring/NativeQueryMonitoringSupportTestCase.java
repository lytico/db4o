/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.optional.monitoring;

import java.util.List;
import javax.management.Notification;
import com.db4o.config.Configuration;
import com.db4o.diagnostic.NativeQueryNotOptimized;
import com.db4o.foundation.ByRef;
import com.db4o.monitoring.*;
import db4ounit.Assert;
import db4ounit.extensions.OptOutNotSupportedJavaxManagement;
import db4ounit.extensions.fixtures.CustomClientServerConfiguration;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class NativeQueryMonitoringSupportTestCase extends QueryMonitoringTestCaseBase implements CustomClientServerConfiguration, OptOutNotSupportedJavaxManagement {
	
	@Override
	protected void configure(Configuration config) throws Exception {
		super.configure(config);
		config.add(new NativeQueryMonitoringSupport());
	}

	public void configureClient(Configuration config) throws Exception {
		configure(config);
	}

	public void configureServer(Configuration config) throws Exception {
	}
	
	public void testUnoptimizedNativeQueriesPerSecond() {
		
		exercisePerSecondCounter("UnoptimizedNativeQueriesPerSecond", new Runnable() { public void run() {
			triggerUnoptimizedQuery();
		}});
		
	}
	
	public void testNativeQueriesPerSecond() {
		
		final ByRef<Boolean> optimized = ByRef.newInstance(true);
		
		exercisePerSecondCounter("NativeQueriesPerSecond", new Runnable() { public void run() {
			if (optimized.value) {
				triggerOptimizedQuery();
			} else {
				triggerUnoptimizedQuery();
			}
			optimized.value = !optimized.value;
		}});
		
	}

	public void testUnoptimizedQueryNotification() throws Exception {
		
		final List<Notification> notifications = startCapturingNotifications(unoptimizedQueryNotificationType());
		
		triggerUnoptimizedQuery();
		
		Assert.areEqual(1, notifications.size());
		
		final Notification notification = notifications.get(0);
		Assert.areEqual(unoptimizedQueryNotificationType(), notification.getType());
		Assert.areEqual(unoptimizableQuery().getClass().getName(), notification.getUserData());
		
	}	

	@Override
	protected Class<?> beanInterface() {
		return NativeQueriesMBean.class;
	}

	protected String unoptimizedQueryNotificationType() {
		return NativeQueryNotOptimized.class.getName();
	}

	@Override
	protected String beanID() {
		return Db4oMBeans.mBeanIDForContainer(isEmbedded() ? fileSession() : db());
	}
}
