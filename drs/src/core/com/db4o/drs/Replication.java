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

import com.db4o.drs.inside.*;
import com.db4o.reflect.*;

/**
 * Factory to create ReplicationSessions.
 *
 * @version 1.3
 * @see com.db4o.drs.hibernate.HibernateReplication
 * @see ReplicationProvider
 * @see ReplicationEventListener
 * @since dRS 1.0
 */
public class Replication {
	
	/**
	 * Begins a replication session between two ReplicationProviders without a 
	 * ReplicationEventListener and with no Reflector provided.
	 *
	 * @throws ReplicationConflictException when conflicts occur
	 * @see ReplicationEventListener
	 */
	public static ReplicationSession begin(
			ReplicationProvider providerA, 
			ReplicationProvider providerB) {
		
		return begin(providerA, providerB, null, null);
	}
	
	/**
	 * Begins a replication session between two ReplicationProviders using a 
	 * ReplicationEventListener and with no Reflector provided.
	 *
	 * @throws ReplicationConflictException when conflicts occur
	 * @see ReplicationEventListener
	 */
	public static ReplicationSession begin(
			ReplicationProvider providerA, 
			ReplicationProvider providerB, 
			ReplicationEventListener listener) {
		
		return begin(providerA, providerB, listener, null);
	}

	
	/**
	 * Begins a replication session between two ReplicationProviders without a 
	 * ReplicationEventListener and with a Reflector provided.
	 *
	 * @throws ReplicationConflictException when conflicts occur
	 * @see ReplicationEventListener
	 */
	public static ReplicationSession begin(
			ReplicationProvider providerFrom, 
			ReplicationProvider providerTo, 
			Reflector reflector) {
		
		return begin(providerFrom, providerTo, null, reflector);
	}

	/**
	 * Begins a replication session between two ReplicationProviders using a 
	 * ReplicationEventListener and with a Reflector provided.
	 *
	 * @throws ReplicationConflictException when conflicts occur
	 * @see ReplicationEventListener
	 */
	public static ReplicationSession begin(
			ReplicationProvider providerFrom, 
			ReplicationProvider providerTo, 
			ReplicationEventListener listener, 
			Reflector reflector) {
		
		if (listener == null) {
			listener = new DefaultReplicationEventListener();
		}
		ReplicationReflector rr = new ReplicationReflector(providerFrom, providerTo, reflector);
		providerFrom.replicationReflector(rr);
		providerTo.replicationReflector(rr);
		return new GenericReplicationSession(providerFrom, providerTo, listener, reflector);
	}


}
