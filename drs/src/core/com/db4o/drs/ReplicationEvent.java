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
 * Defines an event class for the replication of an entity.
 *
 * @since dRS 1.2
 */
public interface ReplicationEvent {
	/**
	 * Does a conflict occur?
	 *
	 * @return true when a conflict occur
	 */
	boolean isConflict();

	/**
	 * Overrides default replication behaviour with some state chosen by the user.
	 *
	 * @param chosen the ObjectState of the prevailing object or null if replication should ignore this object and not traverse to its referenced objects.
	 */
	void overrideWith(ObjectState chosen);

	/**
	 * The ObjectState in provider A.
	 *
	 * @return ObjectState in provider A
	 */
	ObjectState stateInProviderA();

	/**
	 * The ObjectState in provider B.
	 *
	 * @return ObjectState in provider B
	 */
	ObjectState stateInProviderB();

	/**
	 * The time when the object is created in one provider.
	 *
	 * @return time when the object is created in one provider.
	 */
	long objectCreationDate();

	/**
	 * The replication process will not traverse to objects referenced by the current one.
	 */
	void stopTraversal();
	
}
