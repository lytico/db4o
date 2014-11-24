/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.optional.monitoring;

import com.db4o.config.*;
import com.db4o.monitoring.*;

import db4ounit.*;
import db4ounit.extensions.OptOutNotSupportedJavaxManagement;
import db4ounit.extensions.fixtures.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class ReferenceSystemMonitoringSupportTestCase extends MBeanTestCaseBase implements CustomClientServerConfiguration, OptOutDefragSolo, OptOutNotSupportedJavaxManagement {
	
	public static void main(String[] args) {
		new ReferenceSystemMonitoringSupportTestCase().runNetworking();
	}
	
	public static class Item{
		
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		super.configure(config);
		config.add(new ReferenceSystemMonitoringSupport());
	}

	public void configureClient(Configuration config) throws Exception {
		configure(config);
	}

	public void configureServer(Configuration config) throws Exception {
		configure(config);
	}

	@Override
	protected Class<?> beanInterface() {
		return ReferenceSystemMBean.class;
	}

	@Override
	protected String beanID() {
		return Db4oMBeans.mBeanIDForContainer(isEmbedded() ? fileSession() : db());
	}
	
	public void testObjectReferenceCount(){
		int objectCount = 10;
		Item[] items = new Item[objectCount];
		for (int i = 0; i < objectCount; i++) {
			Assert.areEqual(referenceCountForDb4oDatabase() + i, objectReferenceCount());
			items[i] = new Item();
			store(items[i]);
		}
		db().purge(items[0]);
		Assert.areEqual(referenceCountForDb4oDatabase() + objectCount -1, objectReferenceCount());
	}
	
	private Object objectReferenceCount() {
		return bean().getAttribute("ObjectReferenceCount");
	}
	
	private int referenceCountForDb4oDatabase(){
		if(isNetworking()){
			return 0;
		}
		return 1;
	}
	

}
