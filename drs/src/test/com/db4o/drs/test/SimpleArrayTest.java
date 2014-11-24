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

import com.db4o.ObjectSet;
import com.db4o.drs.test.data.*;

import db4ounit.Assert;

public class SimpleArrayTest extends DrsTestCase {

	public void test() {

		storeListToProviderA();

		replicateAllToProviderBFirstTime();

		modifyInProviderB();

		replicateAllStep2();

		addElementInProviderA();

		replicateHolderStep3();
	}

	protected void clean() {delete(new Class[]{SimpleArrayHolder.class, SimpleArrayContent.class});}

	private void storeListToProviderA() {

		SimpleArrayHolder sah = new SimpleArrayHolder("h1");
		SimpleArrayContent sac1 = new SimpleArrayContent("c1");
		SimpleArrayContent sac2 = new SimpleArrayContent("c2");
		sah.add(sac1);
		sah.add(sac2);
		a().provider().storeNew(sah);
		a().provider().commit();

		ensureContent(a(), new String[]{"h1"}, new String[]{"c1", "c2"});
	}

	private void replicateAllToProviderBFirstTime() {
		replicateAll(a().provider(), b().provider());

		ensureContent(a(), new String[]{"h1"}, new String[]{"c1", "c2"});
		ensureContent(b(), new String[]{"h1"}, new String[]{"c1", "c2"});
	}

	private void modifyInProviderB() {

		SimpleArrayHolder sah = (SimpleArrayHolder) getOneInstance(b(), SimpleArrayHolder.class);

		sah.setName("h2");
		SimpleArrayContent sac1 = sah.getArr()[0];
		SimpleArrayContent sac2 = sah.getArr()[1];
		sac1.setName("co1");
		sac2.setName("co2");

		b().provider().update(sac1);
		b().provider().update(sac2);
		b().provider().update(sah);

		b().provider().commit();

		ensureContent(b(), new String[]{"h2"}, new String[]{"co1", "co2"});
	}

	private void replicateAllStep2() {
		replicateAll(b().provider(), a().provider());

		ensureContent(b(), new String[]{"h2"}, new String[]{"co1", "co2"});
		ensureContent(a(), new String[]{"h2"}, new String[]{"co1", "co2"});
	}

	private void addElementInProviderA() {

		SimpleArrayHolder sah = (SimpleArrayHolder) getOneInstance(a(), SimpleArrayHolder.class);
		sah.setName("h3");
		SimpleArrayContent lc3 = new SimpleArrayContent("co3");
		a().provider().storeNew(lc3);
		sah.add(lc3);

		a().provider().update(sah);
		a().provider().commit();

		ensureContent(a(), new String[]{"h3"}, new String[]{"co1", "co2", "co3"});
	}

	private void replicateHolderStep3() {
		replicateClass(a().provider(), b().provider(), SimpleArrayHolder.class);

		ensureContent(a(), new String[]{"h3"}, new String[]{"co1", "co2", "co3"});
		ensureContent(b(), new String[]{"h3"}, new String[]{"co1", "co2", "co3"});
	}

	private void ensureContent(DrsProviderFixture fixture, String[] holderNames, String[] contentNames) {
		int holderCount = holderNames.length;
		int contentCount = contentNames.length;
		ensureInstanceCount(fixture, SimpleArrayHolder.class, holderCount);
		ensureInstanceCount(fixture, SimpleArrayContent.class, contentCount);

		int i = 0;
		ObjectSet objectSet = fixture.provider().getStoredObjects(SimpleArrayHolder.class);
		Iterator iterator = objectSet.iterator();
		while (iterator.hasNext()) {
			SimpleArrayHolder lh = (SimpleArrayHolder) iterator.next();
			Assert.areEqual(holderNames[i], lh.getName());

			SimpleArrayContent[] sacs = lh.getArr();
			for (int j = 0; j < contentNames.length; j++) {
				Assert.areEqual(contentNames[j], sacs[j].getName());
			}
		}
	}

}
