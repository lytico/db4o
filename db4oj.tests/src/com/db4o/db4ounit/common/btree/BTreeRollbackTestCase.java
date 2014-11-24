/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.btree;

import db4ounit.extensions.*;


public class BTreeRollbackTestCase extends BTreeTestCaseBase {

	public static void main(String[] args) {
		new BTreeRollbackTestCase().runSolo();
	}
	
	private static final int[] COMMITTED_VALUES = new int[]{ 6,8,15,45, 43, 9,23, 25,7,3,2};
	
	private static final int[] ROLLED_BACK_VALUES = new int[]{ 16,18,115,19,17,13,12};
	
	public void test(){
		add(COMMITTED_VALUES);
		commitBTree();
		for (int i = 0; i < 5; i++) {
			add(ROLLED_BACK_VALUES);
			rollbackBTree();
		}
		BTreeAssert.assertKeys(trans(), _btree, COMMITTED_VALUES);
	}

	private void commitBTree() {
		_btree.commit(trans());
		trans().commit();
	}

	private void rollbackBTree() {
		_btree.rollback(trans());
		trans().rollback();
	}
	
}
