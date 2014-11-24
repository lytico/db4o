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

import com.db4o.drs.*;
import com.db4o.drs.inside.TestableReplicationProviderInside;
import com.db4o.drs.test.data.*;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;

import db4ounit.Assert;

public class R0to4Runner extends DrsTestCase {

//	 ------------------------------ FIELDS ------------------------------

	private static final int LINKERS = 4;

// --------------------------- CONSTRUCTORS ---------------------------

	public R0to4Runner() {
		super();
	}

	protected void clean() {
		delete(a().provider());
		delete(b().provider());
	}

	protected void delete(TestableReplicationProviderInside provider) {
		ArrayList toDelete = new ArrayList();

		Iterator rr = provider.getStoredObjects(R0.class).iterator();
		while (rr.hasNext()) {
			Object o = rr.next();

			ReflectClass claxx = replicationReflector().forObject(o);
			setFieldsToNull(o, claxx);
			toDelete.add(o);
		}
		
		Object commitObject = null;

		for (Iterator iterator = toDelete.iterator(); iterator.hasNext();) {
			Object o = iterator.next();

			//System.out.println("o = " + o);
			provider.delete(o);
			commitObject = o;
		}
		
		if(commitObject != null){
			provider.commit();
		} else {
			provider.commit();
		}
	}

	private void compareR4(TestableReplicationProviderInside a, TestableReplicationProviderInside b, boolean isSameExpected) {
		Iterator it = a.getStoredObjects(R4.class).iterator();
		while (it.hasNext()) {
			String name = ((R4) it.next()).getName();

			Iterator it2 = b.getStoredObjects(R4.class).iterator();
			boolean found = false;
			while (it2.hasNext()) {
				String name2 = ((R4) it2.next()).getName();
				if (name.equals(name2)) found = true;
			}
			Assert.isTrue(found == isSameExpected);
		}
	}

	private void replicateAllToB(TestableReplicationProviderInside peerA, TestableReplicationProviderInside peerB) {
		Assert.areEqual(LINKERS * 5, replicateAll(peerA, peerB, false));
	}

	private void ensureCount(TestableReplicationProviderInside provider, int linkers) {
		ensureCount(provider, R0.class, linkers * 5);
		ensureCount(provider, R1.class, linkers * 4);
		ensureCount(provider, R2.class, linkers * 3);
		ensureCount(provider, R3.class, linkers * 2);
		ensureCount(provider, R4.class, linkers * 1);
	}

	private void ensureCount(TestableReplicationProviderInside provider, Class clazz, int count) {
		Iterator instances = provider.getStoredObjects(clazz).iterator();
		int i = count;
		while (instances.hasNext()) {
			Object o = instances.next();
			i--;
		}
		Assert.areEqual(0 , i);
	}

	private void ensureR4Different(TestableReplicationProviderInside peerA, TestableReplicationProviderInside peerB) {
		compareR4(peerB, peerA, false);
	}

	private void ensureR4Same(TestableReplicationProviderInside peerA, TestableReplicationProviderInside peerB) {
		compareR4(peerB, peerA, true);
		compareR4(peerA, peerB, true);
	}

	private void init(TestableReplicationProviderInside peerA) {
		R0Linker lCircles = new R0Linker();
		lCircles.setNames("circles");
		lCircles.linkCircles();
		lCircles.store(peerA);

		R0Linker lList = new R0Linker();
		lList.setNames("list");
		lList.linkList();
		lList.store(peerA);

		R0Linker lThis = new R0Linker();
		lThis.setNames("this");
		lThis.linkThis();
		lThis.store(peerA);

		R0Linker lBack = new R0Linker();
		lBack.setNames("back");
		lBack.linkBack();
		lBack.store(peerA);

		peerA.commit();
	}

	private void modifyR4(TestableReplicationProviderInside provider) {
		Object commitObject = null;
		Iterator it = provider.getStoredObjects(R4.class).iterator();
		while (it.hasNext()) {
			R4 r4 = (R4) it.next();
			r4.setName(r4.getName() + "_");
			provider.update(r4);
			commitObject = r4;
		}
		provider.commit();
	}

	private int replicate(TestableReplicationProviderInside peerA, TestableReplicationProviderInside peerB) {
		return replicateAll(peerA, peerB, true);
	}

	private int replicateAll(TestableReplicationProviderInside peerA, TestableReplicationProviderInside peerB, boolean modifiedOnly) {
		ReplicationSession replication = Replication.begin(peerA, peerB, null, _fixtures.reflector);

		Iterator it = modifiedOnly
				? peerA.objectsChangedSinceLastReplication(R0.class).iterator()
				: peerA.getStoredObjects(R0.class).iterator();

		int replicated = 0;
		while (it.hasNext()) {
			R0 r0 = (R0) it.next();
			replication.replicate(r0);
			replicated++;
		}
		replication.commit();

		ensureCount(peerA, LINKERS);
		ensureCount(peerB, LINKERS);
		return replicated;
	}

	private void replicateNoneModified(TestableReplicationProviderInside peerA, TestableReplicationProviderInside peerB) {
		Assert.isTrue(replicate(peerA, peerB) == 0);
	}

	private void replicateR4(TestableReplicationProviderInside peerA, TestableReplicationProviderInside peerB) {
		int replicatedObjectsCount = replicateAll(peerA, peerB, true);
		Assert.areEqual(LINKERS, replicatedObjectsCount);
	}

	private void setFieldsToNull(Object object, ReflectClass claxx) {
		ReflectField[] fields;

		fields = claxx.getDeclaredFields();
		for (int i = 0; i < fields.length; i++) {
			ReflectField field = fields[i];
			if (field.isStatic()) continue;
			if (field.isTransient()) continue;
			field.set(object, null);
		}

		ReflectClass superclass = claxx.getSuperclass();
		if (superclass == null) return;
		setFieldsToNull(object, superclass);
	}

	public void test() {

		init(a().provider());

		ensureCount(a().provider(), LINKERS);

		replicateAllToB(a().provider(), b().provider());

		replicateNoneModified(a().provider(), b().provider());

		modifyR4(a().provider());

		ensureR4Different(a().provider(), b().provider());

		replicateR4(a().provider(), b().provider());

		ensureR4Same(a().provider(), b().provider());
	}

}
