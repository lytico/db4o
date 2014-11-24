/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent.map;

import java.util.*;

import com.db4o.config.*;
import com.db4o.db4ounit.jre12.collections.transparent.*;
import com.db4o.db4ounit.jre12.collections.transparent.map.ActivatableHashMapTestCase.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public abstract class ActivatableMapTestCaseBase extends AbstractDb4oTestCase{

	@Override
	protected void configure(Configuration config) throws Exception {
		config.add(new TransparentPersistenceSupport());
	}
	
	@Override
	protected void store() throws Exception {
		Item<CollectionElement, CollectionElement> item = new Item();
		item._map = newFilledMap();
		store(item);
	}

	protected void assertEqualContent(Map<CollectionElement, CollectionElement> expected, Map<CollectionElement, CollectionElement> actual) {
		
		IteratorAssert.sameContent(expected.keySet().iterator(), actual.keySet().iterator());
		for (CollectionElement key : actual.keySet()) {
			Assert.areEqual(expected.get(key), actual.get(key));
		}
	}

	protected void assertEmpty() {
		assertEqualContent(new HashMap<CollectionElement,CollectionElement>(), singleMap());
	}
	
	protected Item singleItem() {
		return retrieveOnlyInstance(Item.class);
	}
	
	protected Map<CollectionElement,CollectionElement> singleMap(){
		return singleItem()._map;
	}
	
	protected Map<CollectionElement,CollectionElement> newFilledMap() {
		Map<CollectionElement, CollectionElement> map = createMap();
		fillMap(map);
		return map;
	}
	
	protected abstract Map<CollectionElement,CollectionElement> createMap(); 

	private void fillMap(
			Map<CollectionElement, CollectionElement> map) {
		map.put(new Element("plain/plain key"), new Element("plain/plain value"));
		map.put(new Element("plain/activatable key"), new ActivatableElement("plain/activatable value"));
		map.put(new ActivatableElement("activatable/plain key"), new Element("activatable/plain value"));
		map.put(new ActivatableElement("activatable/activatable key"), new ActivatableElement("activatable/activatable value"));
	}

}
