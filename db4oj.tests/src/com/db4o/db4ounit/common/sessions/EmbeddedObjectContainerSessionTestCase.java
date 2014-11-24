/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.sessions;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class EmbeddedObjectContainerSessionTestCase extends AbstractDb4oTestCase implements OptOutMultiSession{
	
	public static class Item {
		
		public String _name;
		
		public Item(String name){
			_name = name;
		}

		@Override
		public boolean equals(Object obj) {
			if (getClass() != obj.getClass())
				return false;
			Item other = (Item) obj;
			if (_name == null) {
				if (other._name != null)
					return false;
			} else if (!_name.equals(other._name))
				return false;
			return true;
		}
		
	}
	
	@Override
	protected void store() throws Exception {
		Item item = new Item("one");
		store(item);
	}
	
	public void testIsolationAgainstMainObjectContainer(){
		assertIsolation(db(), openSession());
	}
	
	public void testIsolationBetweenSessions(){
		assertIsolation(openSession(), openSession());
	}

	private ObjectContainer openSession() {
		return db().ext().openSession();
	}

	private void assertIsolation(ObjectContainer session1,
			ObjectContainer session2) {
		Item originalItem = retrieveItem(session1);
		Item sessionItem = retrieveItem(session2);
		Assert.areNotSame(originalItem, sessionItem);
		Assert.areEqual(originalItem, sessionItem);
	}


	private Item retrieveItem(ObjectContainer session) {
		Query query = session.query();
		query.constrain(Item.class);
		ObjectSet<Item> objectSet = query.execute();
		Item sessionItem = objectSet.next();
		return sessionItem;
	}

}
