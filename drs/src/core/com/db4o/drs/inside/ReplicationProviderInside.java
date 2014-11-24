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
package com.db4o.drs.inside;

import com.db4o.drs.*;
import com.db4o.drs.foundation.*;
import com.db4o.foundation.*;


public interface ReplicationProviderInside extends ReplicationProvider, CollectionSource, SimpleObjectContainer{
	
	/** 
	 * Clear the  ReplicationReference cache
	 */
	void clearAllReferences();

	void commitReplicationTransaction();

	/**
	 * Destroys this provider and frees up resources.
	 */
	void destroy();

	long getLastReplicationVersion();

	String getName();

	ReadonlyReplicationProviderSignature getSignature();
	
	ReplicationReference produceReference(Object obj);

	/**
	 * Collection version of getting a ReplicationReference: 
	 * If the object is not a first class object, we need the
	 * parent object.
	 * @return null, if there is no reference for this object.
	 */
	ReplicationReference produceReference(Object obj, Object parentObject, String fieldNameOnParent);
	
	

	/**
	 * Returns the ReplicationReference of an object by specifying the uuid of the object.
	 *
	 * @param uuid the uuid of the object
	 * @param hint the type of the object
	 * @return the ReplicationReference or null if the reference cannot be found
	 */
	ReplicationReference produceReferenceByUUID(DrsUUID uuid, Class hint);

	ReplicationReference referenceNewObject(Object obj, ReplicationReference counterpartReference, ReplicationReference referencingObjRef, String fieldName);

	/**
	 * Rollbacks all changes done during the replication session  and terminates the Transaction.
	 * Guarantees the changes will not be applied to the underlying databases.
	 */
	void rollbackReplication();

	/**
	 * Start a Replication Transaction with another ReplicationProvider
	 *
	 * @param peerSignature the signature of another ReplicationProvider.
	 */
	void startReplicationTransaction(ReadonlyReplicationProviderSignature peerSignature);

	/**
	 * Stores the new replicated state of obj. It can also be a new object to this
	 * provider.
	 *
	 * @param obj Object with updated state or a clone of new object in the peer.
	 */
	void storeReplica(Object obj);

	/**
	 * Visits the object of each cached ReplicationReference.
	 *
	 * @param visitor implements the visit functions, including copying of object states, and storing of changed objects
	 */
	void visitCachedReferences(Visitor4 visitor);

	boolean wasModifiedSinceLastReplication(ReplicationReference reference);

	void replicateDeletion(DrsUUID uuid);

	Object replaceIfSpecific(Object value);

	boolean isSecondClassObject(Object obj);

	void ensureVersionsAreGenerated();
	
	TimeStamps timeStamps();

	void syncCommitTimestamp(long syncedTimeStamp);

}
