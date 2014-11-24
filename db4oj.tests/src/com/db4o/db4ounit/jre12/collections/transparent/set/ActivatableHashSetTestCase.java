/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.jre12.collections.transparent.set;

import java.util.*;

import com.db4o.activation.*;
import com.db4o.collections.*;
import com.db4o.db4ounit.jre12.collections.transparent.*;

import db4ounit.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public class ActivatableHashSetTestCase extends ActivatableCollectionTestCaseBase<HashSet<CollectionElement>> {
	
	private CollectionSpec<HashSet<CollectionElement>> _spec = 
			new CollectionSpec<HashSet<CollectionElement>>(
					HashSet.class,
					CollectionFactories.activatableHashSetFactory(),
					CollectionFactories.plainHashSetFactory()
			);
	
	public HashSet<CollectionElement> newActivatableCollection() {
		return _spec.newActivatableCollection();
	}
	
	private HashSet<CollectionElement> newPlainSet(){
		return _spec.newPlainCollection();
	}
	
	public void testCreation() {
		new ActivatableHashSet<Object>();
		new ActivatableHashSet<String>(42);
		new ActivatableHashSet<String>(42, 0.001f);
		new ActivatableHashSet<String>(new ArrayList<String>());
	}
	
	public void testClone() throws Exception{
		ActivatableHashSet cloned = (ActivatableHashSet) singleCollection().clone();
		// assert that activator is null - should throw IllegalStateException if it isn't
		cloned.bind(new Activator() {
			public void activate(ActivationPurpose purpose) {
			}
		});
		IteratorAssert.sameContent(newPlainSet().iterator(), cloned.iterator());
	}

	public void testToString(){
		Assert.areEqual(newPlainSet().toString(), singleCollection().toString());
	}

}
