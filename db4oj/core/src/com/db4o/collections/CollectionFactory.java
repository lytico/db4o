/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.collections;

import java.util.*;

import com.db4o.*;
import com.db4o.internal.*;
import com.db4o.internal.collections.*;

/**
 * Collection factory with methods to create collections with behaviour
 * that is optimized for db4o.<br/><br/> 
 * Example usage:<br/>
 * <code>CollectionFactory.forObjectContainer(objectContainer).newBigSet();</code>
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class CollectionFactory {
	
	private final ObjectContainer _objectContainer;
	
	private CollectionFactory(ObjectContainer objectContainer){
		_objectContainer = objectContainer;
	}
	
	/**
	 * returns a collection factory for an ObjectContainer
	 * @param objectContainer - the ObjectContainer
	 * @return the CollectionFactory
	 */
	public static CollectionFactory forObjectContainer(ObjectContainer objectContainer){
		if(isClient(objectContainer)){
			throw new UnsupportedOperationException("CollectionFactory is not yet available for Client/Server.");
		}
		return new CollectionFactory(objectContainer);
	}
	
	/**
	 * creates a new BigSet.<br/><br/>
	 * Characteristics of BigSet:<br/>
	 * - It is optimized by using a BTree of IDs of persistent objects.<br/> 
	 * - It can only hold persistent first class objects (no primitives, no strings, no objects that are not persistent)<br/>
	 * - Objects are activated upon getting them from the BigSet.
	 * <br/><br/>
	 * BigSet is recommend whenever one object references a huge number of other objects and sorting is not required.
	 * @return
	 */
	public <E> Set<E> newBigSet(){
		return new BigSet<E>((LocalObjectContainer) _objectContainer);
	}
	
	private static boolean isClient(ObjectContainer oc){
		return ((InternalObjectContainer)oc).isClient();
	}

}
