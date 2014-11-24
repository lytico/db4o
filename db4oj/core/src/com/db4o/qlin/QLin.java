/* Copyright (C) 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.qlin;

import com.db4o.*;

/**
 * a node in a QLin ("Coolin") query.
 * QLin is a new experimental query interface.
 * We would really like to have LINQ for Java instead. 
 * @since 8.0
 */
public interface QLin<T> {
	
	/**
	 * adds a where node to this QLin query.
	 * @param expression can be any of the following:
	 * 
	 */
	public QLin<T> where(Object expression);
	
	/**
	 * executes the QLin query and returns the result
	 * as an {@link ObjectSet}.
	 * Note that ObjectSet extends List and Iterable
	 * on the platforms that support these interfaces. 
	 * You may want to use these interfaces instead of
	 * working directly against an ObjectSet.
	 */
	// FIXME: The return value should not be as closely bound to db4o.
	// Collection is mutable, it's not nice.
	// Discuss !!!
	public ObjectSet<T> select ();
	
	
	public QLin<T> equal(Object obj);

	public QLin<T> startsWith(String string);

	public QLin<T> limit(int size);

	public QLin<T> smaller(Object obj);

	public QLin<T> greater(Object obj);
	
	
	/**
	 * orders the query by the expression.
	 * Use the {@link QLinSupport#ascending()} and {@link QLinSupport#descending()}
	 * helper methods to set the direction.
	 */
	public QLin<T> orderBy(Object expression, QLinOrderByDirection direction);
	
	public T singleOrDefault(T defaultValue);

	public T single();

}
