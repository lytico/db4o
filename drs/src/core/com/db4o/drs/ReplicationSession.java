/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.drs;

/**
 * Execute a round of replication between two ReplicationProviders.
 * <p/>
 * Sample code of using ReplicationSession:
 * <pre>
 * ReplicationSession session = Replication.begin(objectContainer1, objectContainer2);
 * session.replicate(object);
 * session.commit();
 * session.close();
 * </pre>
 *
 * @version 1.2
 * @see Replication
 * @since dRS 1.0
 */
public interface ReplicationSession {
	
	
	/**
	 * Closes this session and frees used resources. Sessions that were opened
	 * during the creation of ReplicationProviders will be closed as well.
	 * <p/>
	 * Hibernate Sessions created from Hibernate Configurations will be closed.
	 * db4o ObjectContainers will remain open.
	 *
	 * @throws IllegalStateException if the session is still active (neither commitReplicationTransaction() nor rollback is called).
	 */
	public void close();

	/**
	 * Commits replication changes to both ReplicationProviders and marks the
	 * involved ReplicationProviders to be "clean" against each other - to contain
	 * no modified objects.
	 */
	public void commit();

	/**
	 * Returns the ReplicationProvider representing the <b>first</b> persistence
	 * system passed as a parameter when the replication session was instantiated.
	 *
	 * @return the first persistence system
	 */
	public ReplicationProvider providerA();

	/**
	 * Returns the ReplicationProvider representing the <b>second</b> persistence
	 * system passed as a parameter when the replication session was instantiated.
	 *
	 * @return the second persistence system
	 */
	public ReplicationProvider providerB();

	/**
	 * Replicates an individual object and traverses to member objects as long as
	 * members are new or modified since the last time the two ReplicationProviders
	 * were replicated.
	 *
	 * @param obj the object to be replicated.
	 * @see ReplicationEventListener
	 */
	public void replicate(Object obj);

	/**
	 * Replicates all deletions between the two providers.
	 * Cascade delete is disabled, regardless the user's settings.
	 * <p/>
	 * If the deletion violates referential integrity, exception will be thrown.
	 * You can use the dRS replication callback to check whether the object to
	 * be deleted violates referential integrity. If so, you can stop traversal.
	 *
	 * @param extent the Class that you want to delete
	 */
	public void replicateDeletions(Class extent);

	/**
	 * Rollbacks all changes done during the replication session. This call
	 * guarantees the changes will not be applied to the underlying databases. The
	 * state of the objects involved in the replication is undefined.
	 * Both ReplicationProviders may still contain cached references of touched objects.
	 * <p/>
	 * To restart replication, it is recommended to reopen both involved
	 * ReplicationProviders and to start a new ReplicationSession.
	 */
	void rollback();

	/**
	 * Sets the direction of replication. By default, if this method is not called, replication is bidirectional,
	 * which means the newer copy of the object is copied to the other provider..
	 * <p/> If you want to force unidirectional replication, call this method before calling {@link #replicate}.
	 *
	 * @param from objects in this provider will not be changed.
	 * @param to   objects will be copied to this provider if copies in "from" is newer
	 */
	public void setDirection(ReplicationProvider from, ReplicationProvider to);
}
