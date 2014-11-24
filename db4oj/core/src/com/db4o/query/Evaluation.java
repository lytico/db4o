/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.query;

/**
 * For implementation of callback evaluations.
 * <br><br>
 * This is for adding your own constrains to a given query. 
 * Note that evaluations force db4o to instantiate your objects in order
 * to execute the query which slows down to query by an order of magnitude.
 * Pass your implementation of this interface to {@link Query#constrain(Object)}
 * <br><br>
 * Evaluations are called as the last step during query execution,
 * after all other constraints have been applied.
 * <br><br>Client-Server over a network only:<br>
 * Avoid evaluations, because the evaluation object needs to be serialized, which is hard
 * to manage correctly.
 */
public interface Evaluation extends java.io.Serializable {
	
	/**
	 * Callback method during {@link Query#execute() query execution}.
	 * @param candidate reference to the candidate persistent object.
	 */
	public void evaluate(Candidate candidate);
	
}
