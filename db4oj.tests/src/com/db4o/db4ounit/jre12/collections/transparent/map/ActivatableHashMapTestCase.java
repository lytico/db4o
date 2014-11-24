/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent.map;

import java.util.*;

import com.db4o.activation.*;
import com.db4o.collections.*;
import com.db4o.db4ounit.jre12.collections.transparent.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public class ActivatableHashMapTestCase extends ActivatableMapTestCaseBase{

	public static class Item<K,V> {
		public Map<K,V> _map;
	}
	
	public void testCreation() {
		new ActivatableHashMap<CollectionElement, CollectionElement>();
		new ActivatableHashMap<CollectionElement, CollectionElement>(42);
		new ActivatableHashMap<CollectionElement, CollectionElement>(42, (float)0.5);
		HashMap<CollectionElement,CollectionElement> origMap = new HashMap<CollectionElement,CollectionElement>();
		origMap.put(new Element("a"), new Element("b"));
		ActivatableHashMap<CollectionElement, CollectionElement> fromMap = 
			new ActivatableHashMap<CollectionElement, CollectionElement>(origMap);
		assertEqualContent(origMap, fromMap);
	}

	public void testClone() throws Exception{
		ActivatableHashMap cloned = (ActivatableHashMap) ((HashMap)singleMap()).clone();
		// assert that activator is null - should throw IllegalStateException if it isn't
		cloned.bind(new Activator() {
			public void activate(ActivationPurpose purpose) {
			}
		});
		assertEqualContent(newFilledMap(), cloned);
	}
	

	@Override
	protected Map<CollectionElement, CollectionElement> createMap() {
		return new ActivatableHashMap<CollectionElement, CollectionElement>();
	}
	
}
