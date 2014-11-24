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


public class UnqualifiedNamedTestCase extends DrsTestCase {

	public void test() {
		storeInA();
		replicate();
		modifyInB();
		replicate2();
		modifyInA();
		replicate3();
	}
	
	private void storeInA() {
		UnqualifiedNamed unqualifiedNamed = new UnqualifiedNamed("storedInA");
		
		a().provider().storeNew(unqualifiedNamed);
		a().provider().commit();
		
		ensureData(a(), "storedInA");
	}
	
	private void replicate() {
		replicateAll(a().provider(), b().provider());

		ensureData(a(), "storedInA");
		ensureData(b(), "storedInA");
	}
	
	private void modifyInB() {
		UnqualifiedNamed unqualifiedNamed = (UnqualifiedNamed) getOneInstance(b(), UnqualifiedNamed.class);
		unqualifiedNamed.setData("modifiedInB");
		b().provider().update(unqualifiedNamed);
		b().provider().commit();
		ensureData(b(), "modifiedInB");
	}

	private void replicate2() {
		replicateAll(b().provider(), a().provider());
		ensureData(a(), "modifiedInB");
		ensureData(b(), "modifiedInB");
	}

	private void modifyInA() {
		UnqualifiedNamed unqualifiedNamed = (UnqualifiedNamed) getOneInstance(a(), UnqualifiedNamed.class);
		unqualifiedNamed.setData("modifiedInA");
		a().provider().update(unqualifiedNamed);
		a().provider().commit();
		ensureData(a(), "modifiedInA");
	}

	private void replicate3() {
		replicateClass(a().provider(), b().provider(), UnqualifiedNamed.class);

		ensureData(a(), "modifiedInA");
		ensureData(b(), "modifiedInA");
	}

	private void ensureData(DrsProviderFixture fixture, Object data) {
		ensureOneInstance(fixture, UnqualifiedNamed.class);
		UnqualifiedNamed unqualifiedNamed = (UnqualifiedNamed) getOneInstance(fixture, UnqualifiedNamed.class);
		Assert.areEqual(data,unqualifiedNamed.getData());
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