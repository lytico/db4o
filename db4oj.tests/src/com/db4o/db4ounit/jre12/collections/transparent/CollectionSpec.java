/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections.transparent;

import java.util.*;

import com.db4o.foundation.*;
import com.db4o.internal.*;

import db4ounit.fixtures.*;

/**
 * @sharpen.remove
 */
@decaf.Remove(decaf.Platform.JDK11)
public class CollectionSpec<L extends Collection<CollectionElement>> implements Labeled {

	private static String[] NAMES = new String[] {"one", "two", "three"};
	
	private final Closure4<L> _activatableCollectionFactory;
	private final Closure4<L> _plainCollectionFactory;
	private final Class<? super L> _collectionClazz;
	
	public CollectionSpec(
			Class<? super L> collectionClazz, 
			Closure4<L> activatableCollectionFactory,
			Closure4<L> plainCollectionFactory) {
		_activatableCollectionFactory = activatableCollectionFactory;
		_plainCollectionFactory = plainCollectionFactory;
		_collectionClazz = collectionClazz;
	}

	public L newActivatableCollection() {
		L collection = createActivatableCollection();
		for (CollectionElement element: newPlainCollection()) {
			collection.add(element);
		}
		return collection;
	}
	
	public L newPlainCollection(){
		L elements = _plainCollectionFactory.run();
		for (String name  : NAMES) {
			elements.add(new Element(name));
		}
		for (String name  : NAMES) {
			elements.add(new ActivatableElement(name));
		}
		return elements;
	}
	
	public static String firstName() {
		return NAMES[0];
	}
	
	private L createActivatableCollection() {
		return _activatableCollectionFactory.run();
	}

	public String label() {
		return ReflectPlatform.simpleName(_collectionClazz);
	}
}
