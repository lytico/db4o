/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.query;

import com.db4o.*;

/**
 * Candidate for {@link Evaluation} callbacks.
 * <br><br>
 * During {@link Query#execute() query execution} all registered {@link Evaluation} callback
 * handlers are called with {@link Candidate} proxies that represent the persistent objects that
 * meet all other {@link Query} criteria.
 * <br><br>
 * A {@link Candidate} provides access to the query candidate object. It
 * represents and allows to specify whether it is to be included in the query result
 */
public interface Candidate {
	
	/**
	 * Returns the persistent object that is represented by this query
	 * {@link Candidate}.
	 * @return Object the persistent object.
	 */
	public Object getObject();
	
	/**
	 * Specify whether the Candidate is to be included in the result
	 * <br><br>
	 * This method may be called multiple times. The last call prevails.
	 * @param flag true to include that object. False otherwise.
	 */
	public void include(boolean flag);
	
	
	/**
	 * Returns the object container the query is executed on
	 * @return the {@link ObjectContainer}
	 */
	public ObjectContainer objectContainer();
	
}
