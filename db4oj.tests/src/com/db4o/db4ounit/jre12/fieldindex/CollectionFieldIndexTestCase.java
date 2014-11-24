/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre12.fieldindex;

import java.util.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @exclude
 */
/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class CollectionFieldIndexTestCase extends AbstractDb4oTestCase {
	
	public static void main(String[] args) {
		new CollectionFieldIndexTestCase().runSolo();
	}
	
	private static class Item {
		private String _name;
		
		public Item(String name) {
			_name = name;
		}
		
		public String getName() {
			return _name;
		}
	}
	
	private static class UntypedContainer {
		private Object _set = new HashSet();
		
		public UntypedContainer(Object item) {
			((Set)_set).add(item);
		}
		
		public Iterator iterator() {
			return ((Set)_set).iterator();
		}
	}
	
	protected void configure(Configuration config) {
		indexField(config,Item.class, "_name");
		indexField(config,UntypedContainer.class, "_set");
	}
	
	protected void store() throws Exception {
		db().store(new UntypedContainer(new Item("foo")));
		db().store(new UntypedContainer(new Item("bar")));
	}
	
	public void testUntypedContainer() {
		final Query q = db().query();
		q.constrain(UntypedContainer.class);
		q.descend("_set").descend("_name").constrain("foo");
		
		final ObjectSet result = q.execute();
		Assert.areEqual(1, result.size());
		
		final UntypedContainer container = (UntypedContainer)result.next();
		final Item item = (Item)container.iterator().next();
		Assert.areEqual("foo", item.getName());
	}

}
