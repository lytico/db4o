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

import com.db4o.drs.ObjectState;
import com.db4o.drs.ReplicationEvent;
import com.db4o.drs.ReplicationEventListener;
import com.db4o.drs.inside.TestableReplicationProviderInside;
import com.db4o.drs.test.data.*;

import db4ounit.Assert;


public class ReplicationEventTest extends DrsTestCase {

	private static final String IN_A = "in A";
	private static final String MODIFIED_IN_A = "modified in A";
	private static final String MODIFIED_IN_B = "modified in B";

	private void ensureNames(DrsProviderFixture fixture, String parentName, String childName) {
		ensureOneInstanceOfParentAndChild(fixture);
		SPCParent parent = (SPCParent) getOneInstance(fixture, SPCParent.class);

		if (! parent.getName().equals(parentName)) {
			System.out.println("expected = " + parentName);
			System.out.println("actual = " + parent.getName());
		}

		Assert.areEqual(parent.getName(), parentName);
		Assert.areEqual(childName, parent.getChild().getName());
	}

	private void ensureNotExist(TestableReplicationProviderInside provider, Class type) {
		Assert.isTrue(! provider.getStoredObjects(type).iterator().hasNext());
	}

	private void ensureOneInstanceOfParentAndChild(DrsProviderFixture fixture) {
		ensureOneInstance(fixture, SPCParent.class);
		ensureOneInstance(fixture, SPCChild.class);
	}

	private void modifyInProviderA() {
		SPCParent parent = (SPCParent) getOneInstance(a(), SPCParent.class);
		parent.setName(MODIFIED_IN_A);
		SPCChild child = parent.getChild();
		child.setName(MODIFIED_IN_A);
		a().provider().update(parent);
		a().provider().update(child);
		a().provider().commit();

		ensureNames(a(), MODIFIED_IN_A, MODIFIED_IN_A);
	}

	private void modifyInProviderB() {
		SPCParent parent = (SPCParent) getOneInstance(b(), SPCParent.class);
		parent.setName(MODIFIED_IN_B);
		SPCChild child = parent.getChild();
		child.setName(MODIFIED_IN_B);
		b().provider().update(parent);
		b().provider().update(child);
		b().provider().commit();

		ensureNames(b(), MODIFIED_IN_B, MODIFIED_IN_B);
	}

	private void replicateAllToProviderBFirstTime() {
		replicateAll(a().provider(), b().provider());

		ensureNames(a(), IN_A, IN_A);
		ensureNames(b(), IN_A, IN_A);
	}

	private void storeParentAndChildToProviderA() {
		SPCChild child = new SPCChild(IN_A);
		SPCParent parent = new SPCParent(child, IN_A);
		a().provider().storeNew(parent);
		a().provider().commit();

		ensureNames(a(), IN_A, IN_A);
	}

	/*
	private void tstDeletionDefaultPrevail() {
		storeParentAndChildToProviderA();
		replicateAllToProviderBFirstTime();
		deleteInProviderA();

		final BooleanClosure invoked = new BooleanClosure(false);

		ReplicationEventListener listener = new ReplicationEventListener() {
			public void onReplicate(ReplicationEvent event) {
				invoked.setValue(true);

				final ObjectState stateA = event.stateInProviderA();
				final ObjectState stateB = event.stateInProviderB();

				Test.ensure(stateA.getObject() == null);
				Test.ensure(stateB.getObject() != null);

				if (stateB.getObject() instanceof SPCChild && b().provider().supportsCascadeDelete())
					event.stopTraversal();
			}
		};

		replicateAll(a().provider(), b().provider(), listener);

		Test.ensure(invoked.getValue());

		ensureNotExist(a().provider(), SPCParent.class);
		ensureNotExist(a().provider(), SPCChild.class);

		ensureNotExist(b().provider(), SPCParent.class);
		ensureNotExist(b().provider(), SPCChild.class);
	}

	private void tstDeletionNotPrevail() {
		storeParentAndChildToProviderA();
		replicateAllToProviderBFirstTime();
		deleteInProviderA();

		final BooleanClosure invoked = new BooleanClosure(false);

		ReplicationEventListener listener = new ReplicationEventListener() {
			public void onReplicate(ReplicationEvent event) {
				invoked.setValue(true);

				final ObjectState stateA = event.stateInProviderA();
				final ObjectState stateB = event.stateInProviderB();

				Test.ensure(stateA.getObject() == null);
				Test.ensure(stateB.getObject() != null);

				event.overrideWith(stateB);
			}
		};

		replicateAll(a().provider(), b().provider(), listener);

		Test.ensure(invoked.getValue());

		ensureNames(a(), IN_A, IN_A);
		ensureNames(b(), IN_A, IN_A);
	}

	private void tstDeletionOverrideToPrevail() {
		storeParentAndChildToProviderA();
		replicateAllToProviderBFirstTime();
		deleteInProviderA();

		final BooleanClosure invoked = new BooleanClosure(false);

		ReplicationEventListener listener = new ReplicationEventListener() {
			public void onReplicate(ReplicationEvent event) {
				invoked.setValue(true);

				final ObjectState stateA = event.stateInProviderA();
				final ObjectState stateB = event.stateInProviderB();

				Test.ensure(stateA.getObject() == null);
				Test.ensure(stateB.getObject() != null);

				if (stateB.getObject() instanceof SPCChild && b().provider().supportsCascadeDelete())
					event.stopTraversal();
				else
					event.overrideWith(stateA);
			}
		};

		replicateAll(a().provider(), b().provider(), listener);

		Test.ensure(invoked.getValue());

		ensureNotExist(a().provider(), SPCParent.class);
		ensureNotExist(a().provider(), SPCChild.class);

		ensureNotExist(b().provider(), SPCParent.class);
		ensureNotExist(b().provider(), SPCChild.class);
	}
*/
	public void testNewObject() {
		storeParentAndChildToProviderA();

		final BooleanClosure invoked = new BooleanClosure(false);

		ReplicationEventListener listener = new ReplicationEventListener() {
			public void onReplicate(ReplicationEvent event) {
				invoked.setValue(true);

				final ObjectState stateA = event.stateInProviderA();
				final ObjectState stateB = event.stateInProviderB();

				Assert.isTrue(stateA.isNew());
				Assert.isTrue(!stateB.isNew());

				Assert.isNotNull(stateA.getObject());
				Assert.isNull(stateB.getObject());

				event.overrideWith(null);
			}
		};

		replicateAll(a().provider(), b().provider(), listener);

		Assert.isTrue(invoked.getValue());

		ensureNames(a(), IN_A, IN_A);
		ensureNotExist(b().provider(), SPCParent.class);
		ensureNotExist(b().provider(), SPCChild.class);
	}

	public void testNoAction() {
		storeParentAndChildToProviderA();
		replicateAllToProviderBFirstTime();
		modifyInProviderB();

		ReplicationEventListener listener = new ReplicationEventListener() {
			public void onReplicate(ReplicationEvent event) {
				//do nothing
			}
		};
		
		replicateAll(b().provider(), a().provider(), listener);

		ensureNames(a(), MODIFIED_IN_B, MODIFIED_IN_B);
		ensureNames(b(), MODIFIED_IN_B, MODIFIED_IN_B);
	}

	public void testOverrideWhenConflicts() {
		storeParentAndChildToProviderA();
		replicateAllToProviderBFirstTime();

		//introduce conflicts
		modifyInProviderA();
		modifyInProviderB();

		ReplicationEventListener listener = new ReplicationEventListener() {
			public void onReplicate(ReplicationEvent event) {
				Assert.isTrue(event.isConflict());

				if (event.isConflict())
					event.overrideWith(event.stateInProviderB());
			}
		};

		replicateAll(a().provider(), b().provider(), listener);

		ensureNames(a(), MODIFIED_IN_B, MODIFIED_IN_B);
		ensureNames(b(), MODIFIED_IN_B, MODIFIED_IN_B);
	}

	public void testOverrideWhenNoConflicts() {
		storeParentAndChildToProviderA();
		replicateAllToProviderBFirstTime();
		modifyInProviderB();

		ReplicationEventListener listener = new ReplicationEventListener() {
			public void onReplicate(ReplicationEvent event) {
				Assert.isTrue(!event.isConflict());
				event.overrideWith(event.stateInProviderB());
			}
		};

		replicateAll(b().provider(), a().provider(), listener);

		ensureNames(a(), IN_A, IN_A);
		ensureNames(b(), IN_A, IN_A);
	}

	public void testStopTraversal() {
		storeParentAndChildToProviderA();
		replicateAllToProviderBFirstTime();

		//introduce conflicts
		modifyInProviderA();
		modifyInProviderB();

		ReplicationEventListener listener = new ReplicationEventListener() {
			public void onReplicate(ReplicationEvent event) {
				Assert.isTrue(event.isConflict());

				event.overrideWith(null);
			}
		};

		replicateAll(a().provider(), b().provider(), listener);

		ensureNames(a(), MODIFIED_IN_A, MODIFIED_IN_A);
		ensureNames(b(), MODIFIED_IN_B, MODIFIED_IN_B);
	}

	static class BooleanClosure {
		private boolean value;

		public BooleanClosure(boolean value) {
			this.value = value;		}

		void setValue(boolean v) {
			value = v;
		}

		public boolean getValue() {
			return value;
		}
	}
}
