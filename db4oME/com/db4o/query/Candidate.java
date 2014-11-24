/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.query;

import com.db4o.*;

/**
 * candidate for {@link Evaluation} callbacks.
 * <br><br>
 * During {@link Query#execute() query execution} all registered {@link Evaluation} callback
 * handlers are called with  {@link Candidate} proxies that represent the persistent objects that
 * meet all other {@link Query} criteria.
 * <br><br>
 * A {@link Candidate} provides access to the persistent object it
 * represents and allows to specify, whether it is to be included in the 
 * {@link ObjectSet} resultset.
 */
public interface Candidate {
	
	/**
	 * returns the persistent object that is represented by this query 
	 * {@link Candidate}.
	 * @return Object the persistent object.
	 */
	public Object getObject();
	
	/**
	 * specify whether the Candidate is to be included in the 
	 * {@link ObjectSet} resultset.
	 * <br><br>
	 * This method may be called multiple times. The last call prevails.
	 * @param flag inclusion.
	 */
	public void include(boolean flag);
	
	
	/**
	 * returns the {@link ObjectContainer} the Candidate object is stored in.
	 * @return the {@link ObjectContainer}
	 */
	public ObjectContainer objectContainer();
	
}
