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
import com.db4o.config.*;
import com.db4o.drs.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.data.*;

import db4ounit.*;

public abstract class DrsTestCase implements TestCase, TestLifeCycle {
	
	public static final Class[] mappings;
	public static final Class[] extraMappingsForCleaning = 
		new Class[]{
			List.class,
			Map.class,
		};

	static {
		mappings = new Class[]{
				ItemWithCloneable.class,
				ItemWithUntypedField.class,
				NamedList.class,
				Car.class, 
				CollectionHolder.class,
				ItemDates.class,
				ListContent.class,
				ListHolder.class, 
				MapContent.class,
				Pilot.class, 
				R0.class,
				Replicated.class,
				SimpleArrayContent.class, 
				SimpleArrayHolder.class,
				SimpleItem.class,
				SimpleListHolder.class, 
				SPCChild.class,
				SPCParent.class,
				UnqualifiedNamed.class,
				MapHolder.class,
				NewPilot.class,
				IByteArrayHolder.class,
				ActivatableItem.class,
				SimpleEnumContainer.class
		};
	}

	protected final DrsFixture _fixtures = DrsFixtureVariable.value();
	private ReplicationReflector _reflector;
	
	public void setUp() throws Exception {
		cleanBoth();
		configureBoth();
		openBoth();
		store();
		reopen();
	}

	private void cleanBoth() {
		a().clean();
		b().clean();
	}

	protected void clean() {
		for (int i = 0; i < mappings.length; i++) {
			a().provider().deleteAllInstances(mappings[i]);
			b().provider().deleteAllInstances(mappings[i]);
		}

		for (int i = 0; i < extraMappingsForCleaning.length; i++) {
			a().provider().deleteAllInstances(extraMappingsForCleaning[i]);
			b().provider().deleteAllInstances(extraMappingsForCleaning[i]);
		}

		a().provider().commit();
		b().provider().commit();
	}

	protected void store() {}
	
	private void configureBoth() {
		configureInitial(_fixtures.a);
		configureInitial(_fixtures.b);
	}

	private void configureInitial(DrsProviderFixture fixture) {
		Configuration config = db4oConfiguration(fixture);
		if(config == null) {
			return;
		}
		config.generateUUIDs(ConfigScope.GLOBALLY);
		config.generateCommitTimestamps(true);
		configure(config);
	}

	private Configuration db4oConfiguration(DrsProviderFixture fixture) {
		if(!(fixture instanceof Db4oDrsFixture)) {
			return null;
		}
		return ((Db4oDrsFixture)fixture).config();
	}
	
	protected void configure(Configuration config) {
	}

	protected void reopen() throws Exception {
		closeBoth();
		openBoth();
	}

	private void openBoth() throws Exception {
		a().open();
		b().open();
		_reflector = new ReplicationReflector(a().provider(), b().provider(), _fixtures.reflector);
		a().provider().replicationReflector(_reflector);
		b().provider().replicationReflector(_reflector);
	}
	
	public void tearDown() throws Exception {
		closeBoth();
		cleanBoth();
	}

	private void closeBoth() throws Exception {
		a().close();
		b().close();
	}
	
	public DrsProviderFixture a() {
		return _fixtures.a;
	}

	public DrsProviderFixture b() {
		return _fixtures.b;
	}

	protected void ensureOneInstance(DrsProviderFixture fixture, Class clazz) {
		ensureInstanceCount(fixture, clazz, 1);
	}

	protected void ensureInstanceCount(DrsProviderFixture fixture, Class clazz, int count) {
		ObjectSet objectSet = fixture.provider().getStoredObjects(clazz);
		Assert.areEqual(count, objectSet.size());
	}

	protected Object getOneInstance(DrsProviderFixture fixture, Class clazz) {
		Iterator objectSet = fixture.provider().getStoredObjects(clazz).iterator();
		Object candidate = null;
		if (objectSet.hasNext()) {
			candidate = objectSet.next();
			if (objectSet.hasNext()){
				throw new RuntimeException("Found more than one instance of + " + clazz + " in provider = " + fixture);
			}
		}
		return candidate;
	}

	protected void replicateAll(TestableReplicationProviderInside providerFrom, TestableReplicationProviderInside providerTo) {
		final ReplicationSession replication = Replication.begin(providerFrom, providerTo, _fixtures.reflector);
		final ObjectSet changedSet = providerFrom.objectsChangedSinceLastReplication();
		if (changedSet.size() == 0)
			throw new RuntimeException("Can't find any objects to replicate");
		
		replicateAll(replication, changedSet.iterator());
	}

	protected void replicateAll(final ReplicationSession replication, final Iterator allObjects) {
		while (allObjects.hasNext()) {
			Object changed = allObjects.next();
//			System.out.println("Replicating = " + changed);
			replication.replicate(changed);
		}
		replication.commit();
	}
	
	protected void replicateAll(
			TestableReplicationProviderInside from, TestableReplicationProviderInside to, ReplicationEventListener listener) {
		ReplicationSession replication = Replication.begin(from, to, listener, _fixtures.reflector);

		replicateAll(replication, from.objectsChangedSinceLastReplication().iterator());
	}

	protected void delete(Class[] classes) {
		for (int i = 0; i < classes.length; i++) {
			a().provider().deleteAllInstances(classes[i]);
			b().provider().deleteAllInstances(classes[i]);
		}
		
		a().provider().commit();
		b().provider().commit(); 
	}

	protected void replicateClass(TestableReplicationProviderInside providerA, TestableReplicationProviderInside providerB, Class clazz) {
		ReplicationSession replication = Replication.begin(providerA, providerB, null, _fixtures.reflector);
		Iterator allObjects = providerA.objectsChangedSinceLastReplication(clazz).iterator();
		replicateAll(replication, allObjects);
	}

	protected static void sleep(int millis) {
		try {
			Thread.sleep(millis);
		} catch (InterruptedException e) {
			throw new RuntimeException(e.toString());
		}
	}

	protected ReplicationReflector replicationReflector() {
		return _reflector;
	}

	public static void updateTo(DrsProviderFixture fixture, Object... objs) {
		TestableReplicationProviderInside provider = fixture.provider();
		for(Object obj : objs) {
			provider.update(obj);
		}
		provider.commit();
	}

	public static void storeNewTo(DrsProviderFixture fixture, Object... objs) {
		TestableReplicationProviderInside provider = fixture.provider();
		for(Object obj : objs) {
			provider.storeNew(obj);
		}
		provider.commit();
	}
}
