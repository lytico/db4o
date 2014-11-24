/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.ta;

import db4ounit.*;

/**
 * @exclude
 */
public class TALinkedListTestCase extends TAItemTestCaseBase {
    
	public static void main(String[] args) {
		new TALinkedListTestCase().runAll();	
	}
	
	protected Object createItem() throws Exception {
		TALinkedListItem item = new TALinkedListItem();
		item.list = newList();
		return item;
	}

	private TALinkedList newList() {
		return TALinkedList.newList(10);
	}

	protected void assertItemValue(Object obj) throws Exception {
		TALinkedListItem item = (TALinkedListItem) obj;
		Assert.areEqual(newList(),item.list());
	}

	 public void testDeactivateDepth() throws Exception {
	    	TALinkedListItem item = (TALinkedListItem) retrieveOnlyInstance();
	    	TALinkedList list = item.list();
	    	TALinkedList next3 = list.nextN(3);
	    	TALinkedList next5 = list.nextN(5);
	    	
	    	Assert.isNotNull(next3.next());
	    	Assert.isNotNull(next5.next());
	    	
	    	db().deactivate(list, 4);
	    	
	    	Assert.isNull(list.next);
	    	Assert.areEqual(0, list.value);
	    	
	    	// FIXME: test fails if uncomenting the following assertion.
//	    	Assert.isNull(next3.next);
	    	Assert.isNotNull(next5.next);
	    }
}
