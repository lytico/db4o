/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections;

import java.util.*;

import com.db4o.*;
import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

@decaf.Ignore(decaf.Platform.JDK11)
public class CascadeDeleteCollectionTestCase extends AbstractDb4oTestCase{
	
	public static class Item {
		public List _list;
	}
	
	public class Element{
		public String _name;
		
		public Element(String name){
			_name = name;
		}
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.objectClass(Item.class).objectField("_list").cascadeOnDelete(true);
	}
	
	public void store(){
		Item item = new Item();
		item._list = new ArrayList();
		item._list.add(new Element("1"));
		store(item);
	}
	
	public void test(){
		Item item = retrieveOnlyInstance(Item.class);
		db().delete(item);
		db().commit();
		ObjectSet objectSet = db().query(Element.class);
		Assert.areEqual(0, objectSet.size());
	}
	
}
