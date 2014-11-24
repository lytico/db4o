/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.fieldindex;

import com.db4o.internal.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.fixtures.*;

/**
 * @exclude
 */
public class StringIndexTestCase extends StringIndexTestCaseBase implements OptOutMultiSession {
	
	public static void main(String[] args) {
		new StringIndexTestCase().runSolo();
	}
    
    public void testNotEquals() {
    	add("foo");
    	add("bar");
    	add("baz");
    	add(null);
    	
    	final Query query = newQuery(Item.class);
    	query.descend("name").constrain("bar").not();
		assertItems(new String[] { "foo", "baz", null }, query.execute());
    }

	public void testCancelRemovalRollback() throws Exception {
    	
    	prepareCancelRemoval(trans(), "original");
    	rename("original", "updated");
    	db().rollback();
    	grafittiFreeSpace();
    	reopen();
    	
    	assertExists("original");
    }
    
    public void testCancelRemovalRollbackForMultipleTransactions() throws Exception {
    	final Transaction trans1 = newTransaction();
    	final Transaction trans2 = newTransaction();
        
        prepareCancelRemoval(trans1, "original");
        assertExists(trans2, "original");
    	
        trans1.rollback();
        assertExists(trans2, "original");
        
        add(trans2, "second");
        assertExists(trans2, "original");
        
        trans2.commit();
        assertExists(trans2, "original");
        
    	grafittiFreeSpace();
        reopen();
        assertExists("original");
    }
    
    public void testCancelRemoval() throws Exception {
    	prepareCancelRemoval(trans(), "original");
    	db().commit();
    	grafittiFreeSpace();
    	reopen();
    	
    	assertExists("original");
    }

	private void prepareCancelRemoval(Transaction transaction, String itemName) {
		add(itemName);    	
    	db().commit();
    	
    	rename(transaction, itemName, "updated");    	
    	assertExists(transaction, "updated");
    	
    	rename(transaction, "updated", itemName);
    	assertExists(transaction, itemName);
	}
    
    public void testCancelRemovalForMultipleTransactions() throws Exception {
    	final Transaction trans1 = newTransaction();
    	final Transaction trans2 = newTransaction();
    	
    	prepareCancelRemoval(trans1, "original");
    	rename(trans2, "original", "updated");    	
    	trans1.commit();
    	grafittiFreeSpace();
    	reopen();
    	
    	assertExists("original");
    }
    
    public void testDeletingAndReaddingMember() throws Exception{
		add("original");
    	assertExists("original");
        rename("original", "updated");        
        assertExists("updated");
        Assert.isNull(query("original"));
        reopen();        
        assertExists("updated");
        Assert.isNull(query("original"));
    }
}
