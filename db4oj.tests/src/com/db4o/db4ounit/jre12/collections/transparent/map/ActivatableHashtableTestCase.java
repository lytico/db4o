/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent.map;

import java.util.*;

import com.db4o.activation.*;
import com.db4o.collections.*;
import com.db4o.db4ounit.jre12.collections.transparent.*;

import db4ounit.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public class ActivatableHashtableTestCase extends ActivatableMapTestCaseBase{
	
	public void testCreation() {
		new ActivatableHashtable<CollectionElement, CollectionElement>();
		new ActivatableHashtable<CollectionElement, CollectionElement>(42);
		new ActivatableHashtable<CollectionElement, CollectionElement>(42, (float)0.5);
		Hashtable<CollectionElement,CollectionElement> origMap = new Hashtable<CollectionElement,CollectionElement>();
		origMap.put(new Element("a"), new Element("b"));
		ActivatableHashtable<CollectionElement, CollectionElement> fromMap = 
			new ActivatableHashtable<CollectionElement, CollectionElement>(origMap);
		assertEqualContent(origMap, fromMap);
	}

	public void testClone() throws Exception{
		ActivatableHashtable cloned = (ActivatableHashtable) singleHashtable().clone();
		// assert that activator is null - should throw IllegalStateException if it isn't
		cloned.bind(new Activator() {
			public void activate(ActivationPurpose purpose) {
			}
		});
		assertEqualContent(newFilledMap(), cloned);
	}
	
	public void testContains() {
		Hashtable<CollectionElement, CollectionElement> actual = singleHashtable();
		for (CollectionElement expectedValue : newFilledMap().values()) {
			Assert.isTrue(actual.contains(expectedValue));
		}
	}
	
	public void testElements(){
		Collection actual = elementsToCollection(singleHashtable());
		Collection expected = elementsToCollection((Hashtable) newFilledMap());
		IteratorAssert.sameContent(expected, actual);
	}
	
	

	private Collection elementsToCollection(Hashtable hashtable) {
		Collection temp = new ArrayList();
		Enumeration elements = hashtable.elements();
		while(elements.hasMoreElements()){
			temp.add(elements.nextElement());
		}
		return temp;
	}

	@Override
	protected Map<CollectionElement, CollectionElement> createMap() {
		return new ActivatableHashtable<CollectionElement, CollectionElement>();
	}
	
	private Hashtable singleHashtable(){
		return (Hashtable) singleMap();
	}

	

}
