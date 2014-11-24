/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.classindex;

import com.db4o.internal.classindex.*;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class ClassIndexTestCase extends AbstractDb4oTestCase implements OptOutMultiSession{
	
	public static class Item {
		public String name;

		public Item(String _name) {
			this.name = _name;
		}
	}
	
	public static void main(String[] args) {
		new ClassIndexTestCase().runSolo();
	}
	
	public void testDelete() throws Exception {
		Item item = new Item("test");
		store(item);
		int id=(int)db().getID(item);
		assertID(id);

		reopen();
		
		item=(Item)db().queryByExample(item).next();
		id=(int)db().getID(item);
		assertID(id);
		
		db().delete(item);
		db().commit();
		assertEmpty();
		
		reopen();

		assertEmpty();
	}

	private void assertID(int id) {
		assertIndex(new Object[]{new Integer(id)});
	}

	private void assertEmpty() {
		assertIndex(new Object[]{});
	}

	private void assertIndex(Object[] expected) {
		ExpectingVisitor visitor = new ExpectingVisitor(expected);
		ClassIndexStrategy index = classMetadataFor(Item.class).index();
		index.traverseIds(trans(),visitor);
		visitor.assertExpectations();
	}
}
