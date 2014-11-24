/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.optional.monitoring;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.monitoring.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.OptOutNotSupportedJavaxManagement;
import db4ounit.extensions.fixtures.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class ObjectLifecycleMonitoringSupportTestCase extends MBeanTestCaseBase implements CustomClientServerConfiguration, OptOutNotSupportedJavaxManagement {
	
	public static class Item {
		
		public Item _child;
		
		public Item(Item child) {
			_child = child;
		}
		
	}

	public void testObjectsStored(){
		Assert.areEqual(0.0, bean().getAttribute("ObjectsStoredPerSec"));
		Item item = new Item(new Item(null));
		store(item);
		_clock.advance(1000);
		Assert.areEqual(2.0, bean().getAttribute("ObjectsStoredPerSec"));
		store(item);
		_clock.advance(1000);
		Assert.areEqual(1.0, bean().getAttribute("ObjectsStoredPerSec"));
	}
	
	public void testObjectsDeleted() {
		Item item = new Item(new Item(null));
		store(item);
		db().commit();
		db().delete(item);
		db().commit();
		_clock.advance(1000);
		Assert.areEqual(2.0, fileSessionBean().getAttribute("ObjectsDeletedPerSec"));
	}
	
	public void testObjectsActivated() throws Exception{
		ObjectSet<Item> objectSet = storedItems();
		while(objectSet.hasNext()){
			objectSet.next();
		}
		_clock.advance(1000);
		Assert.areEqual(2.0, bean().getAttribute("ObjectsActivatedPerSec"));
	}
	
	public void testObjectsDeactivated() throws Exception{
		ObjectSet<Item> objectSet = storedItems();
		while(objectSet.hasNext()){
			db().deactivate(objectSet.next());
		}
		_clock.advance(1000);
		Assert.areEqual(2.0, bean().getAttribute("ObjectsDeactivatedPerSec"));
	}

	private ObjectSet<Item> storedItems() throws Exception {
		Item item = new Item(new Item(null));
		store(item);
		reopen();
		Query query = newQuery(Item.class);
		return query.<Item>execute();
	}

	@Override
	protected String beanID() {
		return Db4oMBeans.mBeanIDForContainer(container());
	}
	
	@Override
	protected Class<?> beanInterface() {
		return ObjectLifecycleMBean.class;
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		super.configure(config);
		config.add(new ObjectLifecycleMonitoringSupport());
		config.objectClass(Item.class).cascadeOnDelete(true);
	}
	
	public void configureClient(Configuration config) throws Exception {
		configure(config);
	}
	
	public void configureServer(Configuration config) throws Exception {
		configure(config);
	}
	
}
