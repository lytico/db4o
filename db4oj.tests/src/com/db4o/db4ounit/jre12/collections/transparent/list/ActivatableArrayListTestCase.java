/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent.list;

import java.util.*;

import com.db4o.activation.*;
import com.db4o.collections.*;
import com.db4o.db4ounit.jre12.collections.transparent.*;

import db4ounit.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public class ActivatableArrayListTestCase extends ActivatableCollectionTestCaseBase<ArrayList<CollectionElement>> {
	
	private CollectionSpec<ArrayList<CollectionElement>> _spec = 
			new CollectionSpec<ArrayList<CollectionElement>>(
					ArrayList.class,
					CollectionFactories.activatableArrayListFactory(),
					CollectionFactories.plainArrayListFactory()
			);
	
	public ArrayList<CollectionElement> newActivatableCollection() {
		return _spec.newActivatableCollection();
	}
	
	private ArrayList<CollectionElement> newPlainList(){
		return _spec.newPlainCollection();
	}
	
	public void testCreation() {
		new ActivatableArrayList<Object>();
		new ActivatableArrayList<Object>(42);
		new ActivatableArrayList<String>((Collection<String>)new ActivatableArrayList<String>());
	}
	
	public void testClone() throws Exception{
		ActivatableArrayList cloned = (ActivatableArrayList) singleCollection().clone();
		// assert that activator is null - should throw IllegalStateException if it isn't
		cloned.bind(new Activator() {
			public void activate(ActivationPurpose purpose) {
			}
		});
		IteratorAssert.areEqual(newPlainList().iterator(), cloned.iterator());
	}

	public void testToString(){
		Assert.areEqual(newPlainList().toString(), singleCollection().toString());
	}
	
	public void testTrimToSize() throws Exception{
		ArrayList<CollectionElement> singleList = singleCollection();
		singleList.trimToSize();
		assertAreEqual(newPlainList(), singleList);
	}
	
	public void testEnsureCapacity(){
		ArrayList<CollectionElement> singleList = singleCollection();
		singleList.ensureCapacity(10);
		assertAreEqual(newPlainList(), singleList);
	}
	
	@decaf.Ignore(unlessCompatible=decaf.Platform.JDK16)
	public void testRetainAll() throws Exception{
		Element element = new Element("first");
		singleCollection().add(element);
		ArrayList<CollectionElement> newPlainList = newPlainList();
		newPlainList.add(element);
		Assert.isTrue(newPlainList.size()>1);
		Collection<CollectionElement> excerpt = new ArrayList<CollectionElement>();
		excerpt.add(element);
		reopen();
		Assert.isTrue(newPlainList.retainAll(excerpt));
		Assert.isTrue(singleCollection().retainAll(excerpt));
		reopen();
		IteratorAssert.areEqual(newPlainList, singleCollection());
	}


}
