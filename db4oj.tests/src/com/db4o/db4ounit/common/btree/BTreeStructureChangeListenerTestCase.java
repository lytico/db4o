/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.btree;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.btree.*;

import db4ounit.*;

/**
 * @exclude
 */
public class BTreeStructureChangeListenerTestCase extends BTreeTestCaseBase {
	
	public void testSplits(){
		final BooleanByRef splitNotified = new BooleanByRef(); 
		BTreeStructureListener listener = new BTreeStructureListener(){
			public void notifySplit(Transaction trans, BTreeNode originalNode, BTreeNode newRightNode){
				Assert.isFalse(splitNotified.value);
				splitNotified.value = true;
			}
			public void notifyDeleted(Transaction trans, BTreeNode node){
				
			}
			public void notifyCountChanged(Transaction trans, BTreeNode node, int diff){
				
			}
		};
		_btree.structureListener(listener);
		for (int i = 0; i < BTREE_NODE_SIZE + 1; i++) {
			add(i);	
		}
		Assert.isTrue(splitNotified.value);
	}
	
	public void testDelete(){
		final IntByRef deletedCount = new IntByRef(); 
		BTreeStructureListener listener = new BTreeStructureListener(){
			public void notifySplit(Transaction trans, BTreeNode originalNode, BTreeNode newRightNode){
				
			}
			public void notifyDeleted(Transaction trans, BTreeNode node){
				deletedCount.value++;
			}
			public void notifyCountChanged(Transaction trans, BTreeNode node, int diff){
				
			}
		};
		_btree.structureListener(listener);
		for (int i = 0; i < BTREE_NODE_SIZE + 1; i++) {
			add(i);	
		}
		
		for (int i = 0; i < BTREE_NODE_SIZE + 1; i++) {
			remove(i);	
		}
		Assert.areEqual(2, deletedCount.value);
	}
	
	public void testItemCountChanged(){
		final IntByRef changedCount = new IntByRef(); 
		BTreeStructureListener listener = new BTreeStructureListener(){
			public void notifySplit(Transaction trans, BTreeNode originalNode, BTreeNode newRightNode){
				
			}
			public void notifyDeleted(Transaction trans, BTreeNode node){
				
			}
			public void notifyCountChanged(Transaction trans, BTreeNode node, int diff){
				changedCount.value = diff;
			}
		};
		_btree.structureListener(listener);
		changedCount.value = 0;
		add(42);
		Assert.areEqual(1, changedCount.value);
		remove(42);
		Assert.areEqual(-1, changedCount.value);
		changedCount.value = 0;
		remove(42);
		Assert.areEqual(0, changedCount.value);
	}


}
