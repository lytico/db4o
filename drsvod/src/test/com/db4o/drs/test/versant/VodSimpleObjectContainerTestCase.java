/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant;

import java.util.*;

import com.db4o.*;
import com.db4o.drs.test.versant.data.*;
import com.versant.odbms.*;
import com.versant.odbms.model.*;

import db4ounit.*;

public class VodSimpleObjectContainerTestCase extends VodProviderTestCaseBase implements TestLifeCycle {
	
	@Override
	public void setUp() {
		super.setUp();
		_vod.startEventProcessor();
	}
	
	@Override
	public void tearDown() {
		super.tearDown();
		_vod.stopEventProcessor();
	}
	
	public void testStoreNew(){
		Item item = new Item("one");
		_provider.storeNew(item);
		_provider.commit();
		assertContent(Item.class, item);
	}
	
	public void testStoredSimpleObjects() {
		ObjectSet storedObjects = _provider.getStoredObjects(Item.class);
		Assert.areEqual(0, storedObjects.size());
		List<Item> items = new ArrayList<Item>();
		for (int i = 0; i < 3; i++) {
			Item item = new Item(String.valueOf(i));
			items.add(item);
			_provider.storeNew(item);
		}
		_provider.commit();
		
		storedObjects = _provider.getStoredObjects(Item.class);
		IteratorAssert.sameContent(items, storedObjects);
		assertContent(Item.class, items);
	}

	public void testStoredCompoundObjects() {
		List<Holder> holders = new ArrayList<Holder>();
		for (int i = 0; i < 3; i++) {
			Item item = new Item(String.valueOf(i));
			Item[] listItems = new Item[3]; 
			for (int j = 0; j < 3; j++) {
				listItems[j] = new Item(String.valueOf(i * j));
			}
			Holder holder = new Holder(item, listItems);
			holders.add(holder);
			_provider.storeNew(holder);
		}
		_provider.commit();
		ObjectSet storedObjects = _provider.getStoredObjects(Holder.class);
		Assert.areEqual(3, storedObjects.size());
		IteratorAssert.sameContent(holders, storedObjects);
	}
	
	public void testDelete(){
		Item item = new Item("one");
		_provider.storeNew(item);
		_provider.commit();
		_provider.delete(item);
		_provider.commit();
		assertContent(Item.class);
	}
	
	public void testUpdate(){
		Item item = new Item("one");
		_provider.storeNew(item);
		_provider.commit();
		assertContent(Item.class, item);
		item.name("updated");
		_provider.commit();
		assertContent(Item.class, item);
	}
	
	public void testDeleteAllInstances(){
		Item item1 = new Item("one");
		_provider.storeNew(item1);
		Item item2 = new Item("two");
		_provider.storeNew(item2);
		_provider.commit();
		assertContent(Item.class, item1, item2);
		_provider.deleteAllInstances(Item.class);
		_provider.commit();
		assertContent(Item.class);
	}
	
	public void testLongChain(){
		final int length = 1000;
		_provider.storeNew(Chain.newChainWithLength(length));
		_provider.commit();
		Chain chain = _jdo.query(Chain.class, "this._id == 0").iterator().next();
		Assert.areEqual(length, chain.length());
	}
	
	private <T> void assertContent(Class<T> type, T...items) {
		assertContent(type, Arrays.asList(items));
	}
	
	private <T> void assertContent(final Class<T> type, final Iterable<T> items) {
		Collection<T> collection = _jdo.query(type);
		for (Object obj : collection) {
			_jdo.refresh(obj);
		}
		IteratorAssert.sameContent(items, collection);
	}
	
	public void testSchema(){
		
		_provider.storeNew(new Item("one"));
		_provider.commit();

		DatastoreManager dm = _vod.createDatastoreManager();
		DatastoreInfo info = dm.getPrimaryDatastoreInfo();
		SchemaEditor editor = dm.getSchemaEditor();
		long[] classLoids = dm.locateAllClasses(info, false);
		for (int i = 0; i < classLoids.length; i++) {
			DatastoreSchemaClass dc = editor.findClass(classLoids[i], info);
			System.out.println(dc.getName());
		}
		dm.close();
	}

}
