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

import com.db4o.drs.test.data.*;

import db4ounit.Assert;

public class SimpleParentChild extends DrsTestCase {

	public void test() {

		storeParentAndChildToProviderA();

		replicateAllToProviderBFirstTime();

		modifyParentInProviderB();

		replicateAllStep2();

		modifyParentAndChildInProviderA();

		replicateParentClassStep3();
	}

	private void ensureNames(DrsProviderFixture fixture, String parentName, String childName) {
		ensureOneInstanceOfParentAndChild(fixture);
		SPCParent parent = (SPCParent) getOneInstance(fixture, SPCParent.class);

		if (! parent.getName().equals(parentName)) {
			System.out.println("expected = " + parentName);
			System.out.println("actual = " + parent.getName());
		}

		Assert.areEqual(parentName, parent.getName());
		Assert.areEqual(childName, parent.getChild().getName());
	}

	private void ensureOneInstanceOfParentAndChild(DrsProviderFixture fixture) {
		ensureOneInstance(fixture, SPCParent.class);
		ensureOneInstance(fixture, SPCChild.class);
	}

	private void modifyParentAndChildInProviderA() {
		SPCParent parent = (SPCParent) getOneInstance(a(), SPCParent.class);
		parent.setName("p3");
		SPCChild child = parent.getChild();
		child.setName("c3");
		a().provider().update(parent);
		a().provider().update(child);
		a().provider().commit();

		ensureNames(a(), "p3", "c3");
	}

	private void modifyParentInProviderB() {
		SPCParent parent = (SPCParent) getOneInstance(b(), SPCParent.class);
		parent.setName("p2");
		b().provider().update(parent);
		b().provider().commit();

		ensureNames(b(), "p2", "c1");
	}

	private void replicateAllStep2() {
		replicateAll(b().provider(), a().provider());

		ensureNames(a(), "p2", "c1");
		ensureNames(b(), "p2", "c1");
	}

	private void replicateAllToProviderBFirstTime() {
		replicateAll(a().provider(), b().provider());

		ensureNames(a(), "p1", "c1");
		ensureNames(b(), "p1", "c1");
	}

	private void replicateParentClassStep3() {
		replicateClass(a().provider(), b().provider(), SPCParent.class);

		ensureNames(a(), "p3", "c3");
		ensureNames(b(), "p3", "c3");
	}

	private void storeParentAndChildToProviderA() {
		SPCChild child = new SPCChild("c1");
		SPCParent parent = new SPCParent(child, "p1");
		a().provider().storeNew(parent);
		a().provider().commit();

		ensureNames(a(), "p1", "c1");
	}

}
