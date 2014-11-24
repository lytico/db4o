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

import java.util.List;
import com.db4o.drs.inside.TestableReplicationProviderInside;
import com.db4o.drs.test.data.*;

import db4ounit.Assert;

@SuppressWarnings("unchecked")
public class ComplexListTestCase extends DrsTestCase {

	public void test() {
		store(a(), createList());
		replicateAndTest(a(), b());
		roundTripTest();
	}
	
	private void roundTripTest() {
		changeInProviderB();
		
		b().provider().commit();
		replicateAndTest(b(), a());
	}

	private void changeInProviderB() {
		SimpleListHolder simpleListHolder = (SimpleListHolder) getOneInstance(b(), SimpleListHolder.class);
		
		SimpleItem fooBaby = new SimpleItem(simpleListHolder, "foobaby");		
		b().provider().storeNew(fooBaby);
		simpleListHolder.add(fooBaby);		
		SimpleItem foo = getItem(simpleListHolder, "foo");
		foo.setChild(fooBaby);
		b().provider().update(foo);
		b().provider().update(simpleListHolder.getList());
		b().provider().update(simpleListHolder);
	}

	private void replicateAndTest(DrsProviderFixture source, DrsProviderFixture target) {
		replicateAll(source.provider(), target.provider());
		ensureContents(target, (SimpleListHolder) getOneInstance(source, SimpleListHolder.class));
	}

	private void store(DrsProviderFixture fixture , SimpleListHolder listHolder) {
		TestableReplicationProviderInside provider = fixture.provider();
		
		provider.storeNew(listHolder);
		
		provider.storeNew(getItem(listHolder, "foo"));
		provider.storeNew(getItem(listHolder, "foobar"));		
		
		provider.commit();
		
		ensureContents(fixture, listHolder);
	}

	private void ensureContents(DrsProviderFixture actualFixture, SimpleListHolder expected) {
		SimpleListHolder actual = (SimpleListHolder) getOneInstance(actualFixture, SimpleListHolder.class);
		
		List expectedList = expected.getList();
		List actualList = actual.getList();
		
		assertListWithCycles(expectedList, actualList);
	}

	private void assertListWithCycles(List expectedList, List actualList) {
		Assert.areEqual(expectedList.size(), actualList.size());
		
		for(int i = 0; i < expectedList.size(); ++i){
			SimpleItem expected = (SimpleItem) expectedList.get(i);
			SimpleItem actual = (SimpleItem) actualList.get(i);
			
			assertItem(expected, actual);
		}
		
		assertCycle(actualList, "foo", "bar", 1);
		assertCycle(actualList, "foo", "foobar", 1);
		assertCycle(actualList, "foo", "baz", 2);		
	}

	private void assertCycle(List list, String childName, String parentName, int level) {
		SimpleItem foo = getItem(list, childName);
		SimpleItem bar = getItem(list, parentName);
		
		Assert.isNotNull(foo);
		Assert.isNotNull(bar);
		
		Assert.areSame(foo, bar.getChild(level));
		Assert.areSame(foo.getParent(), bar.getParent());
	}

	private void assertItem(SimpleItem expected, SimpleItem actual) {
		if (expected == null) {
			Assert.isNull(actual);
			return;
		}
		
		Assert.areEqual(expected.getValue(), actual.getValue());
		assertItem(expected.getChild(), actual.getChild());
	}

	private SimpleItem getItem(SimpleListHolder holder, String tbf) {
		return getItem(holder.getList(), tbf);
	}
	
	private SimpleItem getItem(List list, String tbf) {
		int itemIndex = list.indexOf(new SimpleItem(tbf));		
		return (SimpleItem) (itemIndex >= 0 ? list.get(itemIndex) : null); 
	}

	public SimpleListHolder createList() {
		
		// list : {foo, bar, baz, foobar}
		//
		// baz -----+
		//          |
		//         bar --> foo
		//                  ^
		//                  |
		// foobar ----------+
		
		SimpleListHolder listHolder = new SimpleListHolder("root");
		
		SimpleItem foo = new SimpleItem(listHolder, "foo");
		SimpleItem bar = new SimpleItem(listHolder, foo, "bar");
		listHolder.add(foo);
		listHolder.add(bar);
		listHolder.add(new SimpleItem(listHolder, bar, "baz"));
		listHolder.add(new SimpleItem(listHolder, foo, "foobar"));
		
		return listHolder;
	}
}
