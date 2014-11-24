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
import com.db4o.drs.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.data.*;
import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.fixtures.*;
/** @sharpen.ignore */
public class ReplicationFeaturesMainFixtureBased extends FixtureBasedTestSuite {
	
	private static final boolean DEBUG = false;

	private static FixtureVariable<ProviderSet> _direction = FixtureVariable.newInstance("direction");
	private static FixtureVariable<ProviderSet> _containersToQueryFrom = FixtureVariable.newInstance("query");
	private static FixtureVariable<ProviderSet> _containersWithNewObjects = FixtureVariable.newInstance("new");
	private static FixtureVariable<ProviderSet> _containersWithChangedObjects = FixtureVariable.newInstance("changed");
	private static FixtureVariable<ProviderSet> _containersWithDeletedObjects = FixtureVariable.newInstance("deleted");
	private static FixtureVariable<ProviderSet> _containerStateToPrevail = FixtureVariable.newInstance("prevail");

	@Override
	public Class[] testUnits() {
		return new Class[] {
				ReplicationFeaturesMainUnit.class,
		};
	}

	@Override
	public FixtureProvider[] fixtureProviders() {
		return new FixtureProvider[] {
				new SimpleFixtureProvider<ProviderSet>(
						_direction, 
						ProviderSet.A_ONLY, 
						ProviderSet.B_ONLY, 
						ProviderSet.BOTH
				),
				new SimpleFixtureProvider<ProviderSet>(
						_containersToQueryFrom, 
						ProviderSet.A_ONLY, 
						ProviderSet.B_ONLY, 
						ProviderSet.BOTH
				),
				new SimpleFixtureProvider<ProviderSet>(
						_containersWithNewObjects, 
						ProviderSet.NONE, 
						ProviderSet.A_ONLY, 
						ProviderSet.B_ONLY, 
						ProviderSet.BOTH
				),
				new SimpleFixtureProvider<ProviderSet>(
						_containersWithChangedObjects, 
						ProviderSet.NONE, 
						ProviderSet.A_ONLY, 
						ProviderSet.B_ONLY, 
						ProviderSet.BOTH
				),
				new SimpleFixtureProvider<ProviderSet>(
						_containersWithDeletedObjects, 
						ProviderSet.NONE, 
						ProviderSet.A_ONLY, 
						ProviderSet.B_ONLY, 
						ProviderSet.BOTH
				),
				new SimpleFixtureProvider<ProviderSet>(
						_containerStateToPrevail, 
						ProviderSet.NONE, 
						ProviderSet.A_ONLY, 
						ProviderSet.B_ONLY, 
						null
				),
		};
	}

	public static class ReplicationFeaturesMainUnit extends DrsTestCase {
	
		private static void fail(String string) {
			System.err.println(string);
			throw new RuntimeException(string);
		}
	
		private void replicateQueryingFrom(ReplicationSession replication, ReplicationProvider origin, ReplicationProvider other) {
			ReplicationConflictException exception = null;
	
			Iterator changes = origin.objectsChangedSinceLastReplication().iterator();
			while (changes.hasNext()) {
				Object changed = changes.next();
				try {
					replication.replicate(changed);
				} catch (ReplicationConflictException e) {
					exception = e;
				}
			}
	
			if (exception != null) throw exception;
		}
	
		private boolean isReplicationConflictExceptionExpectedReplicatingModifications() {
			return wasConflictReplicatingModifications() && isDefaultReplicationBehaviorAllowed();
		}
	
		private boolean isReplicationConflictExceptionExpectedReplicatingDeletions() {
			return wasConflictReplicatingDeletions() && isDefaultReplicationBehaviorAllowed();
		}
	
		private boolean wasConflictReplicatingDeletions() {
			if (!ProviderSet.single(containersWithDeletedObjects())) return false;
			ProviderHandle container = firstContainerWithDeletedObjects();
	
			if (hasChanges(other(container))) return true;
	
			if (!ProviderSet.single(direction())) return false;
			return direction().contains(container);
		}
		
		private ProviderHandle firstContainerWithDeletedObjects() {
			return first(containersWithDeletedObjects().iterator());
		}
	
		private boolean isDefaultReplicationBehaviorAllowed() {
			return containerStateToPrevail() != null && containerStateToPrevail() == ProviderSet.NONE;
		}	
	
		private void changeObject(TestableReplicationProviderInside container, String name, String newName) {
			Replicated obj = find(container, name);
			if (obj == null) return;
			obj.setName(newName);
			container.update(obj);
			out("CHANGED: "+container+": "+name+" => "+newName+" - "+obj);
		}
	
		private void checkEmpty(TestableReplicationProviderInside provider) {
			if (provider.getStoredObjects(Replicated.class).iterator().hasNext())
				throw new RuntimeException(provider.getName() + " is not empty");
		}
	
		private int checkNameCount=0;
		
		private void checkName(TestableReplicationProviderInside container, String name, boolean isExpected) {
			out("");
			out(name + (isExpected ? " " : " NOT") + " expected in container " + containerName(container));
			Replicated obj = find(container, name);
			out(String.valueOf(checkNameCount));
			checkNameCount++;
			if (isExpected) {
				Assert.isNotNull(obj, "Expecting: " + name + " in " + containerName(container));
			} else {
				Assert.isNull(obj);
			}
		}
	
		private String containerName(TestableReplicationProviderInside container) {
			return container == a().provider() ? "A" : "B";
		}
	
		private void checkNames() {
			checkNames(ProviderHandle.A, ProviderHandle.A);
			checkNames(ProviderHandle.A, ProviderHandle.B);
			checkNames(ProviderHandle.B, ProviderHandle.A);
			checkNames(ProviderHandle.B, ProviderHandle.B);
		}
	
		private void checkNames(ProviderHandle origin, ProviderHandle inspected) {
			checkName(container(inspected), "oldFrom" + origin, isOldNameExpected(inspected));
			checkName(container(inspected), "newFrom" + origin, isNewNameExpected(origin, inspected));
			checkName(container(inspected), "oldFromAChangedIn" + origin, isChangedNameExpected(origin, inspected));
			checkName(container(inspected), "oldFromBChangedIn" + origin, isChangedNameExpected(origin, inspected));
		}
	
		private TestableReplicationProviderInside container(ProviderHandle aOrB) {
			return aOrB.equals(ProviderHandle.A) ? a().provider() : b().provider();
		}
	
		private void deleteObject(TestableReplicationProviderInside container, String name) {
			Replicated obj = find(container, name);
			container.delete(obj);
		}
	
		private void doIt() {
			initState();
	
			printProvidersContent("before changes");
			
			performChanges();
	
			printProvidersContent("after changes");
	
			ReplicationEventListener listener = new ReplicationEventListener() {
				public void onReplicate(ReplicationEvent e) {
					if (containerStateToPrevail() == null) {
						e.overrideWith(null);
						return;
					}
	
					if (containerStateToPrevail() == ProviderSet.NONE) return;  //Default replication behaviour.
	
					ObjectState override = containerStateToPrevail().contains(ProviderHandle.A)
							? e.stateInProviderA()
							: e.stateInProviderB();
					e.overrideWith(override);
				}
			};
			
			final ReplicationSession replication = new GenericReplicationSession(a().provider(), b().provider(), listener, _fixtures.reflector);
	
			if (ProviderSet.single(direction())) {
				if (direction().contains(ProviderHandle.A))	replication.setDirection(b().provider(), a().provider());
				if (direction().contains(ProviderHandle.B))	replication.setDirection(a().provider(), b().provider());
			}
			out("DIRECTION: "+direction());
			boolean successful = tryToReplicate(replication);
	
			replication.commit();
	
			printProvidersContent("after replication");
			
			if (successful)
				checkNames();
	
			clean();
			
		}
	
		private void printProvidersContent(String msg) {
			if(!DEBUG){
				return;
			}
			System.out.println("*** "+msg);
			printProviderContent(a().provider());
			printProviderContent(b().provider());
		}
		
		private void printProviderContent(TestableReplicationProviderInside provider) {
			ObjectSet storedObjects = provider.getStoredObjects(Replicated.class);
			System.out.println("PROVIDER: "+provider);
			while(storedObjects.hasNext()) {
				Object object = storedObjects.next();
				System.out.println("-> " + object + " - c:" +creationTime(provider, object) + " v:" + version(provider, object));
			}
		}
	
		private long version(TestableReplicationProviderInside provider, Object obj) {
			return provider.objectVersion(obj);
		}
	
		private long creationTime(TestableReplicationProviderInside provider, Object obj) {
			return provider.creationTime(obj);
		}

		private boolean tryToReplicate(final ReplicationSession replication) {
	
			try {
				replicate(replication, ProviderHandle.A);
				replicate(replication, ProviderHandle.B);
				Assert.isFalse(isReplicationConflictExceptionExpectedReplicatingModifications());
			} catch (ReplicationConflictException e) {
				out("Conflict exception during modification replication.");
				Assert.isTrue(isReplicationConflictExceptionExpectedReplicatingModifications());
				return false;
			}
	
			try {
				if (isDeletionReplicationTriggered())
					replication.replicateDeletions(Replicated.class);
	
				Assert.isFalse(isReplicationConflictExceptionExpectedReplicatingDeletions());
			} catch (ReplicationConflictException e) {
				out("Conflict exception during deletion replication.");
				Assert.isTrue(isReplicationConflictExceptionExpectedReplicatingDeletions());
				return false;
			}
	
			return true;
		}
	
		private void replicate(final ReplicationSession replication, ProviderHandle originName) {
			ReplicationProvider origin = container(originName);
			ReplicationProvider destination = container(other(originName));
	
			if (!containersToQueryFrom().contains(originName)) return;
	
			replicateQueryingFrom(replication, origin, destination);
		}
	
		private Replicated find(TestableReplicationProviderInside container, String name) {
			Iterator storedObjects = container.getStoredObjects(Replicated.class).iterator();
	
			int resultCount = 0;
			Replicated result = null;
			while (storedObjects.hasNext()) {
				Replicated replicated = (Replicated) storedObjects.next();
				if (replicated == null)
					throw new RuntimeException();
				if (name.equals(replicated.getName())) {
					result = replicated;
					resultCount++;
				}
			}
	
			if (resultCount > 1)
				fail("At most one object with name " + name + " was expected.");
			return result;
		}
	
		private boolean hasChanges(ProviderHandle container) {
			return containersWithChangedObjects().contains(container);
		}
	
		private boolean hasDeletions(ProviderHandle container) {
			return containersWithDeletedObjects().contains(container);
		}
	
		private void initState() {
			checkEmpty(a().provider());
			checkEmpty(b().provider());
	
			a().provider().storeNew(new Replicated("oldFromA"));
			b().provider().storeNew(new Replicated("oldFromB"));
	
			a().provider().commit();
			b().provider().commit();
			
			printProvidersContent("init state");
			final ReplicationSession replication = new GenericReplicationSession(a().provider(), b().provider());
	
			replicateQueryingFrom(replication, a().provider(), b().provider());
			replicateQueryingFrom(replication, b().provider(), a().provider());
	
			replication.commit();
		}
	
	
		private boolean isChangedNameExpected(ProviderHandle changedContainer, ProviderHandle inspectedContainer) {
			if (!hasChanges(changedContainer)) return false;
			if (isDeletionExpected(inspectedContainer)) return false;
			if (isDeletionExpected(changedContainer)) return false;
	
			if (inspectedContainer.equals(changedContainer))
				return !didReceiveRemoteState(inspectedContainer);
	
			return didReceiveRemoteState(inspectedContainer);
		}
	
	
		private boolean didReceiveRemoteState(ProviderHandle inspectedContainer) {
			ProviderHandle other = other(inspectedContainer);
	
			if (isDirectionTo(other)) return false;
	
			if (containerStateToPrevail() == null) return false;
	
			if (containerStateToPrevail().contains(inspectedContainer)) return false;
	
			if (containerStateToPrevail().contains(other)) {
				if (isModificationReplicationTriggered()) return true;
				if (isDeletionReplicationTriggered()) return true;
				return false;
			}
	
			//No override to prevail. Default replication behavior.
	
			if (hasChanges(inspectedContainer)) return false; //A conflict would have been ignored long ago.
	
			return isModificationReplicationTriggered();
		}
	
	
		private boolean isDeletionReplicationTriggered() {
			return containersWithDeletedObjects() != ProviderSet.NONE;
		}
	
		private boolean isDirectionTo(ProviderHandle container) {
			return ProviderSet.single(direction()) && direction().contains(container);
		}
	
		private boolean wasConflictReplicatingModifications() {
			return wasConflictWhileReplicatingModificationsQueryingFrom(ProviderHandle.A) || wasConflictWhileReplicatingModificationsQueryingFrom(ProviderHandle.B);
		}
	
	
		private boolean isModificationReplicationTriggered() {
			return wasModificationReplicationTriggeredQueryingFrom(ProviderHandle.A) || wasModificationReplicationTriggeredQueryingFrom(ProviderHandle.B);
		}
	
		private boolean isDeletionExpected(ProviderHandle inspectedContainer) {
			if (containerStateToPrevail() == null)
				return hasDeletions(inspectedContainer);
	
			if (containerStateToPrevail().contains(inspectedContainer))
				return hasDeletions(inspectedContainer);
	
			ProviderHandle other = other(inspectedContainer);
			if (isDirectionTo(other)) return hasDeletions(inspectedContainer);
	
			if (containerStateToPrevail().contains(other))
				return hasDeletions(other);
	
			//_containerStateToPrevail is empty (default replication behaviour)
			return isDeletionReplicationTriggered();
		}
	
		private boolean isNewNameExpected(ProviderHandle origin, ProviderHandle inspected) {
			if (!containersWithNewObjects().contains(origin)) return false;
			if (origin.equals(inspected)) return true;
	
			if (containerStateToPrevail() == null) return false;
			if (containerStateToPrevail().contains(inspected)) return false;
	
			if (!containersToQueryFrom().contains(origin)) return false;
			return direction().contains(inspected);
		}
	
		private boolean isOldNameExpected(ProviderHandle inspectedContainer) {
			if (isDeletionExpected(inspectedContainer)) return false;
			if (isChangedNameExpected(ProviderHandle.A, inspectedContainer)) return false;
			if (isChangedNameExpected(ProviderHandle.B, inspectedContainer)) return false;
			return true;
		}
	
	
		private ProviderHandle other(ProviderHandle aOrB) {
			return aOrB.equals(ProviderHandle.A) ? ProviderHandle.B : ProviderHandle.A;
		}
	
		private void performChanges() {
			if (containersWithNewObjects().contains(ProviderHandle.A)) {
				a().provider().storeNew(new Replicated("newFromA"));
			}
			if (containersWithNewObjects().contains(ProviderHandle.B)) {
				b().provider().storeNew(new Replicated("newFromB"));
			}
	
			if (hasDeletions(ProviderHandle.A)) {
				deleteObject(a().provider(), "oldFromA");
				deleteObject(a().provider(), "oldFromB");
			}
			if (hasDeletions(ProviderHandle.B)) {
				deleteObject(b().provider(), "oldFromA");
				deleteObject(b().provider(), "oldFromB");
			}
	
			if (hasChanges(ProviderHandle.A)) {
				changeObject(a().provider(), "oldFromA", "oldFromAChangedInA");
				changeObject(a().provider(), "oldFromB", "oldFromBChangedInA");
			}
			if (hasChanges(ProviderHandle.B)) {
				changeObject(b().provider(), "oldFromA", "oldFromAChangedInB");
				changeObject(b().provider(), "oldFromB", "oldFromBChangedInB");
			}
	
			a().provider().commit();
			b().provider().commit();
		}
	
		private ProviderHandle first(Iterator4 iterator) {
			return (ProviderHandle)Iterators.next(iterator);
		}
	
		public void test() {
			clean();
			doIt();
		}
	
		private static void out(String string) {
			if (DEBUG) {
				System.out.println(string);
			}
		}

		private boolean wasConflictWhileReplicatingModificationsQueryingFrom(ProviderHandle container) {
			if (!wasModificationReplicationTriggeredQueryingFrom(container)) return false;
			if (ProviderSet.containsAll(containersWithChangedObjects(), direction())) return true;
			return hasDeletions(other(container));
		}
	
		private boolean wasModificationReplicationTriggeredQueryingFrom(ProviderHandle container) {
			if (!containersToQueryFrom().contains(container)) return false;
			if (containersWithDeletedObjects().contains(container)) return false;
			return containersWithChangedObjects().contains(container);
		}	
	}

	private static ProviderSet containersWithDeletedObjects() {
		return _containersWithDeletedObjects.value();
	}

	private static ProviderSet direction() {
		return _direction.value();
	}

	private static ProviderSet containerStateToPrevail() {
		return _containerStateToPrevail.value();
	}

	private static ProviderSet containersToQueryFrom() {
		return _containersToQueryFrom.value();
	}

	private static ProviderSet containersWithChangedObjects() {
		return _containersWithChangedObjects.value();
	}

	private static ProviderSet containersWithNewObjects() {
		return _containersWithNewObjects.value();
	}

	private final static class ProviderHandle {
		private String id;
		
		public static ProviderHandle A = new ProviderHandle("A");
		public static ProviderHandle B = new ProviderHandle("B");
		
		private ProviderHandle(String id) {
			this.id = id;
		}
		
		public String toString() {
			return id;
		}
	}

	private abstract static class ProviderSet implements Labeled {
		public final static ProviderSet NONE = new ProviderSet("NONE") {
			public boolean contains(ProviderHandle provider) {
				return false;
			}

			public Iterator4 iterator() {
				return Iterators.EMPTY_ITERATOR;
			}
		};
		
		public final static ProviderSet A_ONLY = new ProviderSet("A_ONLY") {
			public boolean contains(ProviderHandle provider) {
				return provider == ProviderHandle.A;
			}

			public Iterator4 iterator() {
				return Iterators.singletonIterator(ProviderHandle.A);
			}
			
		};
		public final static ProviderSet B_ONLY = new ProviderSet("B_ONLY") {
			public boolean contains(ProviderHandle provider) {
				return provider == ProviderHandle.B;
			}
			
			public Iterator4 iterator() {
				return Iterators.singletonIterator(ProviderHandle.B);
			}
		};
		public final static ProviderSet BOTH = new ProviderSet("BOTH") {
			public boolean contains(ProviderHandle provider) {
				return true;
			}
			
			public Iterator4 iterator() {
				return Iterators.iterate(ProviderHandle.A, ProviderHandle.B);
			}
		};
		
		public abstract boolean contains(ProviderHandle provider);
		public abstract Iterator4 iterator();

		public String label() {
			return name;
		}
		
		public static boolean single(ProviderSet ps) {
			return ps == A_ONLY || ps == B_ONLY;
		}

		private String name;
		
		private ProviderSet(String name) {
			this.name = name;
		}
		
		public static boolean containsAll(ProviderSet sup, ProviderSet sub) {
			Iterator4 iter = sub.iterator();
			while(iter.moveNext()) {
				if(!sup.contains((ProviderHandle) iter.current())) {
					return false;
				}
			}
			return true;
		}
	}
	
}
