/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre11.assorted;

import java.util.*;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CascadeToHashtableTestCase extends AbstractDb4oTestCase{
	
	public static class Item{
		public Hashtable ht;
	}
	
	public static class Element{
		
		public Element _child;
		
		public String _name;
		
		public Element(String name) {
			_name = name;
		}

		public Element(Element child, String name) {
			_child = child;
			_name = name;
		}
		
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.objectClass(Item.class).cascadeOnUpdate(true);
		config.objectClass(Item.class).cascadeOnDelete(true);
	}
	
	@Override
	protected void store() throws Exception {
		Item item = new Item();
		item.ht = new Hashtable();
		item.ht.put("key1", new Element("stored1"));
		item.ht.put("key2", new Element(new Element("storedChild1"), "stored2"));
		store(item);
	}

	public void test() throws Exception {
		Item item = retrieveOnlyInstance(Item.class);
		item.ht.put("key1", new Element("updated1"));
		Element element = (Element)item.ht.get("key2"); 
		element._name = "updated2";
		store(item);
		
		reopen();
		
		item = retrieveOnlyInstance(Item.class);
		
		element = (Element)item.ht.get("key1");
		Assert.areEqual("updated1", element._name);
		element = (Element)item.ht.get("key2");
		Assert.areEqual("updated2", element._name);
		
		reopen();
		item = retrieveOnlyInstance(Item.class);
		db().delete(item);
		
		Assert.areEqual(2, db().query(Element.class).size());
	}

}
