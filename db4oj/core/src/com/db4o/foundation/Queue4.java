/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.foundation;


public interface Queue4<E> {

	void add(E obj);

	E next();

	boolean hasNext();
	
	/**
	 * Returns the next object in the queue that matches the specified condition.
	 * 
	 * The operation is always NON-BLOCKING.
	 *  
	 * @param condition the object must satisfy to be returned
	 * 
	 * @return the object satisfying the condition or null if none does
	 */
	E nextMatching(Predicate4<E> condition);

	Iterator4 iterator();
}