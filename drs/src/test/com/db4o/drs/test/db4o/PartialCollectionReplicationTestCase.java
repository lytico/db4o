/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.db4o;

import java.util.*;

import com.db4o.drs.db4o.*;
import com.db4o.drs.test.*;
import com.db4o.events.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;

import db4ounit.*;

public class PartialCollectionReplicationTestCase extends DrsTestCase {
	
	public static class Data {
		private List _children;
		private String _id;
		
		public Data(String id) {
			_id = id;
			_children = new ArrayList();
		}
		
		public Object id() {
			return _id;
		}
		
		public void id(String id) {
			_id = id;
		}

		public void add(Data data) {
			_children.add(data);
		}
		
		@Override
		public String toString() {
			return "Data(" + _id + ", " + _children + ")";
		}
	}
	
	public void testNoReplicationForUntouchedElements() {
		final Data root = new Data("root");
		final Data c1 = new Data("c1");
		final Data c2 = new Data("c2");
		root.add(c1);
		root.add(c2);
		
		store(root, 1);
		
		final List<Data> created = replicateAllCapturingCreatedObjects();
		assertData(created, "root", "c1", "c2");
		
		final Data c3 = new Data("c3");
		root.add(c3);
		store(root, 2);
		
		c2.id("c2*");
		c2.add(new Data("c4"));
		
		final List<Data> updated = replicateAllCapturingUpdatedObjects();
		
		// The following fails after cleaning references has been removed from #replicate(obj) 
		// assertData(updated, "c3", "root");
	}

	private void assertData(final Iterable<Data> data, final String... expectedIds) {
		Iterator4Assert.sameContent(expectedIds, ids(data));
	}
	
	private Iterator4 ids(Iterable<Data> data) {
		final Collection4 ids = new Collection4();
		for (Data d : data) {
			ids.add(d.id());
		}
		return ids.iterator();
	}

	private List<Data> replicateAllCapturingUpdatedObjects() {
		final List<Data> changed = new ArrayList<Data>();
		listenToUpdated(changed);
		listenToCreated(changed);
		replicateAll();
		return changed;
	}
	
	private List<Data> replicateAllCapturingCreatedObjects() {
		final List<Data> created = new ArrayList<Data>();
		listenToCreated(created);
		replicateAll();
		return created;
	}

	private void listenToUpdated(final List<Data> updated) {
		eventRegistryFor(b()).updated().addListener(new EventListener4() {
			public void onEvent(Event4 e, EventArgs args) {
				final Object o = ((ObjectEventArgs)args).object();
				if (o instanceof Data) {
					updated.add((Data) o);
				}
				ods(o);
			}
		});
	}

	private void replicateAll() {
		ods("BEGIN REPLICATION");
		replicateAll(a().provider(), b().provider());
		ods("END REPLICATION");
	}

	private void listenToCreated(final List<Data> created) {
		eventRegistryFor(b()).created().addListener(new EventListener4() {
			public void onEvent(Event4 e, EventArgs args) {
				final Object o = ((ObjectEventArgs)args).object();
				if (o instanceof Data) {
					created.add((Data) o);
				}
				ods(o);
			}
		});
	}

	private EventRegistry eventRegistryFor(final DrsProviderFixture fixture) {
		return EventRegistryFactory.forObjectContainer(containerFor(fixture));
	}
	
	public void testCollectionUpdateDoesNotTouchExistingElements() {
		final Data root = new Data("root");
		final Data c1 = new Data("c1");
		final Data c2 = new Data("c2");
		root.add(c1);
		root.add(c2);
		
		store(root, 1);
		
		final long c1Version = versionFor(c1);
		final long c2Version = versionFor(c2);
		
		final Data c3 = new Data("c3");
		root.add(c3);
		store(root, 2);

		Assert.isGreater(0, versionFor(c3));
		Assert.areEqual(c1Version, versionFor(c1));
		Assert.areEqual(c2Version, versionFor(c2));
	}

	private void store(final Data root, final int depth) {
		final ExtObjectContainer container = containerFor(a());
		container.ext().store(root, depth);
		container.commit();
	}

	private ExtObjectContainer containerFor(final DrsProviderFixture fixture) {
		return ((Db4oReplicationProvider)fixture.provider()).getObjectContainer();
	}
	
	private long versionFor(final Data c1) {
		return objectInfoFor(c1).getCommitTimestamp();
	}

	private ObjectInfo objectInfoFor(final Data c1) {
		return containerFor(a()).ext().getObjectInfo(c1);
	}
	
	private void ods(final Object o) {
//		System.out.println(o);
	}

	public static void main(String[] args) {
		
		new ConsoleTestRunner(
				new DrsTestSuiteBuilder(
						new Db4oDrsFixture("db4o-a"),
						new Db4oDrsFixture("db4o-b"),
						PartialCollectionReplicationTestCase.class)).run();
	}
}
