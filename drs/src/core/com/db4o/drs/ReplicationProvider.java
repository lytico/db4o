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

import com.db4o.*;
import com.db4o.drs.inside.*;

/**
 * Facade for persistence systems that provide replication support.
 * Interacts with another ReplicationProvider and a  ReplicationSession
 * to allows replication of objects between two ReplicationProviders.
 * <p/>
 * <p/> To create an instance of this class, use the methods of {@link Replication}.
 *
 * @see ReplicationSession
 * @see Replication
 * @since dRS 1.0
 */
public interface ReplicationProvider {
	/**
	 * Returns newly created objects and changed objects since last replication with the opposite provider.
	 *
	 * @return newly created objects and changed objects since last replication with the opposite provider.
	 */
	ObjectSet objectsChangedSinceLastReplication();

	/**
	 * Returns newly created objects and changed objects since last replication with the opposite provider.
	 *
	 * @param clazz the type of objects interested
	 * @return newly created objects and changed objects of the type specified in the clazz parameter since last replication
	 */
	ObjectSet objectsChangedSinceLastReplication(Class clazz);
	
	void replicationReflector(ReplicationReflector replicationReflector);

}
