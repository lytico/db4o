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

import com.db4o.drs.test.data.*;

import db4ounit.Assert;

/**
 * @sharpen.ignore
 */
public class ListTest extends DrsTestCase {

	public void test() {
		actualTest();
	}
	
	protected void actualTest() {

		storeListToProviderA();

		replicateAllToProviderBFirstTime();

		modifyInProviderB();

		replicateAllStep2();

		addElementInProviderA();

		replicateHolderStep3();
	}

	private void storeListToProviderA() {

		ListHolder lh = createHolder();
		ListContent lc1 = new ListContent("c1");
		ListContent lc2 = new ListContent("c2");
		lh.add(lc1);
		lh.add(lc2);
		a().provider().storeNew(lh);
		a().provider().commit();

		ensureContent(a(), new String[]{"h1"}, new String[]{"c1", "c2"});
	}

	protected ListHolder createHolder() {
		ListHolder lh = new ListHolder("h1");
		lh.setList(new ArrayList());
		return lh;
	}

	private void replicateAllToProviderBFirstTime() {
		replicateAll(a().provider(), b().provider());

		ensureContent(a(), new String[]{"h1"}, new String[]{"c1", "c2"});
		ensureContent(b(), new String[]{"h1"}, new String[]{"c1", "c2"});
	}

	private void modifyInProviderB() {

		ListHolder lh = (ListHolder) getOneInstance(b(), ListHolder.class);

		lh.setName("h2");
		Iterator itor = lh.getList().iterator();
		ListContent lc1 = (ListContent) itor.next();
		ListContent lc2 = (ListContent) itor.next();
		lc1.setName("co1");
		lc2.setName("co2");

		b().provider().update(lc1);
		b().provider().update(lc2);
		b().provider().update(lh.getList());
		b().provider().update(lh);

		b().provider().commit();

		ensureContent(b(), new String[]{"h2"}, new String[]{"co1", "co2"});
	}

	private void replicateAllStep2() {
		replicateAll(b().provider(), a().provider());

		ensureContent(b(), new String[]{"h2"}, new String[]{"co1", "co2"});
		ensureContent(a(), new String[]{"h2"}, new String[]{"co1", "co2"});
	}

	private void addElementInProviderA() {

		ListHolder lh = (ListHolder) getOneInstance(a(), ListHolder.class);
		lh.setName("h3");
		ListContent lc3 = new ListContent("co3");
		a().provider().storeNew(lc3);
		lh.getList().add(lc3);

		a().provider().update(lh.getList());
		a().provider().update(lh);
		a().provider().commit();

		ensureContent(a(), new String[]{"h3"}, new String[]{"co1", "co2", "co3"});
	}

	private void replicateHolderStep3() {
		replicateClass(a().provider(), b().provider(), ListHolder.class);

		ensureContent(a(), new String[]{"h3"}, new String[]{"co1", "co2", "co3"});
		ensureContent(b(), new String[]{"h3"}, new String[]{"co1", "co2", "co3"});
	}

	private void ensureContent(DrsProviderFixture fixture, String[] holderNames, String[] contentNames) {
		int holderCount = holderNames.length;
		ensureInstanceCount(fixture, ListHolder.class, holderCount);

		// After dropping generating uuid for collection, it does not
		//  make sense to count collection because collection is never reused
		//	ensureInstanceCount(provider, ArrayList.class, holderCount);

		int i = 0;
		Iterator objectSet = fixture.provider().getStoredObjects(ListHolder.class).iterator();
		while (objectSet.hasNext()) {
			ListHolder lh = (ListHolder) objectSet.next();
			Assert.areEqual(holderNames[i], lh.getName());

			Iterator itor = lh.getList().iterator();
			//FIXME
			
			int idx = 0;
			while (itor.hasNext()){
				Assert.areEqual(contentNames[idx], ((ListContent) itor.next()).getName());
				idx++;
			}
		}
	}
}
