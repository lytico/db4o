/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class PeekPersistedTestCase extends AbstractDb4oTestCase implements OptOutStackOverflowIssue {
	
	public static final class Item {
		
	    public String name;
	    
	    public Item child;
	    
	    public Item() {
	    }
	    
	    public Item(String name, Item child) {
	    	this.name = name;
	    	this.child = child;
	    }
	    
	    public String toString() {
	    	return "Item(" + name + ", " + child + ")";
	    }
	}
    
    protected void store() {
        final Item root = new Item("1", null);
        Item current = root;
        for (int i = 2; i < 11; i++) {
            current.child = new Item("" + i, null);
            current = current.child;
        }
        store(root);
    }
    
    public void test(){
        Item root = queryRoot();
        for (int i = 0; i < 10; i++) {
            peek(root, i);
        }
    }

	private Item queryRoot() {
		Query q = newQuery(Item.class);
        q.descend("name").constrain("1");
        ObjectSet objectSet = q.execute();
        return (Item)objectSet.next();
	}
    
    private void peek(Item original, int depth){
        Item peeked = (Item)db().peekPersisted(original, depth, true);
        for (int i = 0; i <= depth; i++) {
            Assert.isNotNull(peeked, "Failed to peek at child " + i + " at depth " + depth);
            Assert.isFalse(db().isStored(peeked));
            peeked = peeked.child;
        }
        Assert.isNull(peeked);
    }

}
