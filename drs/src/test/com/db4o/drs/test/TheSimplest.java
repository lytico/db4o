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
package com.db4o.drs.test;

import java.util.Iterator;

import com.db4o.drs.Replication;
import com.db4o.drs.ReplicationSession;
import com.db4o.drs.inside.TestableReplicationProviderInside;
import com.db4o.drs.test.data.*;

import db4ounit.Assert;


public class TheSimplest extends DrsTestCase {

	public void test() {
		storeInA();
		replicate();
		modifyInB();
		replicate2();
		modifyInA();
		replicate3();
	}
	
	private void storeInA() {
		String name = "c1";
		SPCChild child = createChildObject(name);
		
		a().provider().storeNew(child);
		a().provider().commit();
		
		ensureNames(a(), "c1");
	}
	
	private void replicate() {
		replicateAll(a().provider(), b().provider());

		ensureNames(a(), "c1");
		ensureNames(b(), "c1");
	}
	
	private void modifyInB() {
		SPCChild child = getTheObject(b());

		child.setName("c2");
		b().provider().update(child);
		b().provider().commit();

		ensureNames(b(), "c2");
	}

	private void replicate2() {
		replicateAll(b().provider(), a().provider());

		ensureNames(a(), "c2");
		ensureNames(b(), "c2");
	}

	private void modifyInA() {
		SPCChild child = getTheObject(a());

		child.setName("c3");

		a().provider().update(child);
		a().provider().commit();

		ensureNames(a(), "c3");
	}


	private void replicate3() {
		replicateClass(a().provider(), b().provider(), SPCChild.class);

		ensureNames(a(), "c3");
		ensureNames(b(), "c3");
	}

	protected SPCChild createChildObject(String name) {
		return new SPCChild(name);
	}
	
	private void ensureNames(DrsProviderFixture fixture, String childName) {
		ensureOneInstance(fixture, SPCChild.class);
		SPCChild child = getTheObject(fixture);
		Assert.areEqual(childName,child.getName());
	}

	private SPCChild getTheObject(DrsProviderFixture fixture) {
		return (SPCChild) getOneInstance(fixture, SPCChild.class);
	}

	protected void replicateClass(TestableReplicationProviderInside providerA, TestableReplicationProviderInside providerB, Class clazz) {
		ReplicationSession replication = Replication.begin(providerA, providerB, _fixtures.reflector);
		Iterator allObjects = providerA.objectsChangedSinceLastReplication(clazz).iterator();
		while (allObjects.hasNext()) {
			final Object obj = allObjects.next();
			//System.out.println("obj = " + obj);
			replication.replicate(obj);
		}
		replication.commit();
	}

}