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

import java.util.*;

import com.db4o.*;
import com.db4o.drs.foundation.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.data.*;
import com.db4o.ext.*;

import db4ounit.*;


public class ReplicationProviderTest extends DrsTestCase {

	protected byte[] B_SIGNATURE_BYTES;
	protected ReadonlyReplicationProviderSignature B_SIGNATURE;
	private ReadonlyReplicationProviderSignature A_SIGNATURE;

	public void testReplicationLifeCycle() {
		B_SIGNATURE_BYTES = b().provider().getSignature().getSignature();

		A_SIGNATURE = a().provider().getSignature();
		B_SIGNATURE = b().provider().getSignature();

		tstObjectUpdate();

		tstSignature();

		tstObjectsChangedSinceLastReplication();

		tstReferences();

		tstStore();

		tstRollback();

		tstDeletion();
	}


	protected void tstDeletion() {
		a().provider().storeNew(new Pilot("Pilot1", 42));
		Pilot o = new Pilot("Pilot2", 43);
		a().provider().storeNew(o);

		a().provider().commit();

		a().provider().storeNew(new Pilot("Pilot3", 44));

		a().provider().delete(findPilot("Pilot1"));

		Car car = new Car("Car1");
		car.setPilot(findPilot("Pilot2"));
		a().provider().storeNew(car);

		a().provider().commit();

		startReplication();

		DrsUUID uuidCar1 = uuid(findCar("Car1"));
		Assert.isNotNull(uuidCar1);

		a().provider().replicateDeletion(uuidCar1);

		commitReplication();

		Assert.isNull(findCar("Car1"));

		startReplication();

		DrsUUID uuidPilot2 = uuid(findPilot("Pilot2"));
		Assert.isNotNull(uuidPilot2);
		a().provider().replicateDeletion(uuidPilot2);

		commitReplication();

		Assert.isNull(findPilot("Pilot2"));

	}

	private void commitReplication() {
		a().provider().commitReplicationTransaction();
		b().provider().commitReplicationTransaction();
	}

	private Object findCar(String model) {
		Iterator cars = a().provider().getStoredObjects(Car.class).iterator();
		while (cars.hasNext()) {
			Car candidate = (Car) cars.next();
			if (candidate.getModel().equals(model)) return candidate;
		}
		return null;
	}

	private Pilot findPilot(String name) {
		Iterator pilots = a().provider().getStoredObjects(Pilot.class).iterator();
		while (pilots.hasNext()) {
			Pilot candidate = (Pilot) pilots.next();
			if (candidate.name().equals(name)) return candidate;
		}
		return null;
	}

	private SPCChild getOneChildFromA() {
		return getOneChild(a());
	}

	private SPCChild getOneChild(DrsProviderFixture fixture) {
		ObjectSet storedObjects = fixture.provider().getStoredObjects(SPCChild.class);
		Assert.areEqual(1, storedObjects.size());
		
		Iterator iterator = storedObjects.iterator();
		Assert.isTrue(iterator.hasNext());
		return (SPCChild) iterator.next();
	}

	private void startReplication() {
		a().provider().startReplicationTransaction(B_SIGNATURE);
		b().provider().startReplicationTransaction(A_SIGNATURE);
	}

	private void tstObjectUpdate() {
		SPCChild child = new SPCChild("c1");
		a().provider().storeNew(child);
		a().provider().commit();

		startReplication();
		SPCChild reloaded = getOneChildFromA();
		long oldVer = a().provider().produceReference(reloaded, null, null).version();
		commitReplication();

		SPCChild reloaded2 = getOneChildFromA();
		reloaded2.setName("c3");

		//System.out.println("==============BEGIN DEBUG");
		a().provider().update(reloaded2);
		a().provider().commit();
		//System.out.println("==============END DEBUG");

		startReplication();
		SPCChild reloaded3 = getOneChildFromA();
		long newVer = a().provider().produceReference(reloaded3, null, null).version();
		commitReplication();

		Assert.isGreater(oldVer, newVer);
	}

	private void tstObjectsChangedSinceLastReplication() {
		Pilot object1 = new Pilot("John Cleese", 42);
		Pilot object2 = new Pilot("Terry Gilliam", 53);
		Car object3 = new Car("Volvo");

		a().provider().storeNew(object1);
		a().provider().storeNew(object2);
		a().provider().storeNew(object3);

		a().provider().commit();

		startReplication();

		ObjectSet changed = a().provider().objectsChangedSinceLastReplication();
		Assert.areEqual(3, changed.size());

		ObjectSet os = a().provider().objectsChangedSinceLastReplication(Pilot.class);
		Assert.areEqual(2, os.size());
		
		Iterator pilots = os.iterator();
//		Assert.isTrue(pilots.contains(findPilot("John Cleese")));
	//	Assert.isTrue(pilots.contains(findPilot("Terry Gilliam")));
		
		Iterator cars = a().provider().objectsChangedSinceLastReplication(Car.class).iterator();		
		Assert.areEqual("Volvo", ((Car) next(cars)).getModel());
		Assert.isFalse(cars.hasNext());

		commitReplication();

		startReplication();

		Assert.isFalse(a().provider().objectsChangedSinceLastReplication().iterator().hasNext());
		commitReplication();

		Pilot pilot = (Pilot) next(a().provider().getStoredObjects(Pilot.class).iterator());
		pilot.setName("Terry Jones");

		Car car = (Car) next(a().provider().getStoredObjects(Car.class).iterator());
		car.setModel("McLaren");

		a().provider().update(pilot);
		a().provider().update(car);

		a().provider().commit();

		startReplication();

		Assert.areEqual(2, a().provider().objectsChangedSinceLastReplication().size());

		pilots = a().provider().objectsChangedSinceLastReplication(Pilot.class).iterator();
		Assert.areEqual("Terry Jones", ((Pilot) next(pilots)).name());
		Assert.isFalse(pilots.hasNext());

		cars = a().provider().objectsChangedSinceLastReplication(Car.class).iterator();		
		Assert.areEqual("McLaren", ((Car) next(cars)).getModel());
		Assert.isFalse(cars.hasNext());
		commitReplication();

		a().provider().deleteAllInstances(Pilot.class);
		a().provider().deleteAllInstances(Car.class);
		a().provider().commit();
	}


	private Object next(Iterator iterator) {
		Assert.isTrue(iterator.hasNext());
		return iterator.next();
	}

	private void tstReferences() {
		Pilot pilot = new Pilot("tst References", 42);
		a().provider().storeNew(pilot);
		a().provider().commit();

		startReplication();

		Pilot object1 = (Pilot) next(a().provider().getStoredObjects(Pilot.class).iterator());

		ReplicationReference reference = a().provider().produceReference(object1, null, null);
		Assert.areEqual(object1, reference.object());

		DrsUUID uuid = reference.uuid();
		ReplicationReference ref2 = a().provider().produceReferenceByUUID(uuid, Pilot.class);
		Assert.areEqual(reference, ref2);

		a().provider().clearAllReferences();
		DrsUUID db4oUUID = a().provider().produceReference(object1, null, null).uuid();
		Assert.areEqual(uuid, db4oUUID);
		commitReplication();

		a().provider().deleteAllInstances(Pilot.class);
		a().provider().commit();
	}

	private void tstRollback() {
		if (!a().provider().supportsRollback()) return;
		if (!b().provider().supportsRollback()) return;

		startReplication();

		Pilot object1 = new Pilot("Albert Kwan", 25);
		DrsUUID uuid = new DrsUUIDImpl(new Db4oUUID(5678, B_SIGNATURE_BYTES));

		ReplicationReference ref = new ReplicationReferenceImpl(object1, uuid, 1);
		a().provider().referenceNewObject(object1, ref, null, null);

		a().provider().storeReplica(object1);
		Assert.isFalse(a().provider().wasModifiedSinceLastReplication(ref));
		
		a().provider().rollbackReplication();

		a().provider().startReplicationTransaction(B_SIGNATURE);
		Assert.isNull(a().provider().produceReference(object1, null, null));
		ReplicationReference byUUID = a().provider().produceReferenceByUUID(uuid, object1.getClass());
		Assert.isNull(byUUID);
		
		a().provider().rollbackReplication();
		b().provider().rollbackReplication();
	}

	private void tstSignature() {
		Assert.isNotNull(a().provider().getSignature());
	}

	private void tstStore() {
		startReplication();

		Pilot object1 = new Pilot("John Cleese", 42);
		DrsUUID uuid = new DrsUUIDImpl(new Db4oUUID(15, B_SIGNATURE_BYTES));

		ReplicationReference ref = new ReplicationReferenceImpl("ignoredSinceInOtherProvider", uuid, 1);

		a().provider().referenceNewObject(object1, ref, null, null);
		
		a().provider().storeReplica(object1);
		ReplicationReference reference = a().provider().produceReferenceByUUID(uuid, object1.getClass());
		Assert.areEqual(reference, a().provider().produceReference(object1, null, null));
		Assert.areEqual(object1, reference.object());

		commitReplication();
		startReplication();

		Iterator storedObjects = a().provider().getStoredObjects(Pilot.class).iterator();		
		Pilot reloaded = (Pilot)next(storedObjects);

		Assert.isFalse(storedObjects.hasNext());
		
		reference = a().provider().produceReferenceByUUID(uuid, object1.getClass());
		Assert.areEqual(reference, a().provider().produceReference(reloaded, null, null));

		reloaded.setName("i am updated");
		a().provider().storeReplica(reloaded);

		a().provider().clearAllReferences();

		commitReplication();

		startReplication();

		reference = a().provider().produceReferenceByUUID(uuid, reloaded.getClass());
		Assert.areEqual("i am updated", ((Pilot) reference.object()).name());

		commitReplication();

		a().provider().deleteAllInstances(Pilot.class);
		a().provider().commit();
	}

	private DrsUUID uuid(Object obj) {
		return a().provider().produceReference(obj, null, null).uuid();
	}

}
