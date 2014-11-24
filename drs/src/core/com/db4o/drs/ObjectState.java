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
 * The state of the entity in a provider.
 *
 * @author Albert Kwan
 * @author Klaus Wuestefeld
 * @version 1.2
 * @since dRS 1.2
 */
public interface ObjectState {
	/**
	 * The entity.
	 *
	 * @return null if the object has been deleted or if it was not replicated in previous replications.
	 */
	Object getObject();

	/**
	 * Is the object newly created since last replication?
	 *
	 * @return true when the object is newly created since last replication
	 */
	boolean isNew();

	/**
	 * Was the object modified since last replication?
	 *
	 * @return true when the object was modified since last replication
	 */
	boolean wasModified();

	/**
	 * The time when the object is modified in a provider.
	 *
	 * @return time when the object is modified in a provider.
	 */
	long modificationDate();
	
	/**
	 * whether or not the object is known to the ReplicationProvider.
	 */
	boolean isKnown();
}
