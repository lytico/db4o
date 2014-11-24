/* Copyright (C) 2005   db4objects Inc.   http://www.db4o.com */

package com.db4o.replication;

import com.db4o.*;

/**
 * will be called by a {@link com.db4o.replication.ReplicationProcess}upon
 * replication conflicts. Conflicts occur whenever
 * {@link ReplicationProcess#replicate(Object)}is called with an object that
 * was modified in both ObjectContainers since the last replication run between
 * the two.
 */
public interface ReplicationConflictHandler {

	/**
	 * the callback method to be implemented to resolve a conflict. <br>
	 * <br>
	 * 
	 * @param replicationProcess
	 *            the {@link ReplicationProcess}for which this
	 *            ReplicationConflictHandler is registered
	 * @param a
	 *            the object modified in the peerA ObjectContainer
	 * @param b
	 *            the object modified in the peerB ObjectContainer
	 * @return the object (a or b) that should prevail in the conflict or null,
	 *         if no action is to be taken. If this would violate the direction
	 *         set with
	 *         {@link ReplicationProcess#setDirection(ObjectContainer, ObjectContainer)}
	 *         no action will be taken.
	 * @see ReplicationProcess#peerA()
	 * @see ReplicationProcess#peerB()
	 */
	public Object resolveConflict(ReplicationProcess replicationProcess,
			Object a, Object b);

}
