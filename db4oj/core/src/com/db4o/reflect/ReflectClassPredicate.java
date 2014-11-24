/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.reflect;

/**
 * Predicate representation.
 * @see com.db4o.query.Predicate
 * @see Reflector
 */
public interface ReflectClassPredicate {
    
	/**
	 * Match method definition. Used to select correct 
	 * results from an object set.
	 * @param item item to be matched to the criteria
	 * @return true, if the requirements are met
	 */
    public boolean match(ReflectClass item);

}
