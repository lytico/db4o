/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.query;

/**
 * for implementation of callback evaluations.
 * <br><br>
 * To constrain a {@link Query} node with your own callback
 * <code>Evaluation</code>, construct an object that implements the
 * <code>Evaluation</code> interface and register it by passing it
 * to {@link Query#constrain(Object)}.
 * <br><br>
 * Evaluations are called as the last step during query execution,
 * after all other constraints have been applied. Evaluations in higher
 * level {@link Query} nodes in the query graph are called first.
 * <br><br>Java client/server only:<br>
 * db4o first attempts to use Java Serialization to allow to pass final
 * variables to the server. Please make sure that all variables that are
 * used within the evaluate() method are Serializable. This may include
 * the class an anonymous Evaluation object is created in. If db4o is
 * not successful at using Serialization, the Evaluation is transported
 * to the server in a db4o MemoryFile. In this case final variables can
 * not be restored. 
 */
public interface Evaluation {
	
	/**
	 * callback method during {@link Query#execute() query execution}.
	 * @param candidate reference to the candidate persistent object.
	 */
	public void evaluate(Candidate candidate);
	
}
