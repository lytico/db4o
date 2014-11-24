/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.btree;

import com.db4o.internal.*;
import com.db4o.internal.btree.*;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class BTreeSearchTestCase extends AbstractDb4oTestCase implements
		OptOutDefragSolo, OptOutMultiSession {

	protected static final int BTREE_NODE_SIZE = 4;

	public static void main(String[] arguments) {
		new BTreeSearchTestCase().runSolo();
	}

	public void test() throws Exception {
		cycleIntKeys(new int[] { 3, 5, 7, 10, 11, 12, 14, 15, 17, 20, 21, 25 });
	}

	private void cycleIntKeys(int[] values) throws Exception {
		BTree btree = BTreeAssert.createIntKeyBTree(container(), 0,
				BTREE_NODE_SIZE);
		for (int i = 0; i < 5; i++) {
			btree = cycleIntKeys(btree, values);
		}
	}

	private BTree cycleIntKeys(BTree btree, int[] values) throws Exception {
		for (int i = 0; i < values.length; i++) {
			btree.add(trans(), new Integer(values[i]));
		}
		expectKeysSearch(trans(), btree, values);

		btree.commit(trans());

		int id = btree.getID();

		container().commit(trans());

		reopen();

		btree = BTreeAssert.createIntKeyBTree(container(), id, BTREE_NODE_SIZE);

		expectKeysSearch(trans(), btree, values);

		for (int i = 0; i < values.length; i++) {
			btree.remove(trans(), new Integer(values[i]));
		}

		BTreeAssert.assertEmpty(trans(), btree);

		btree.commit(trans());

		BTreeAssert.assertEmpty(trans(), btree);

		return btree;
	}

	private void expectKeysSearch(Transaction trans, BTree btree, int[] keys) {
		int lastValue = Integer.MIN_VALUE;
		for (int i = 0; i < keys.length; i++) {
			if (keys[i] != lastValue) {
				ExpectingVisitor expectingVisitor = ExpectingVisitor
						.createExpectingVisitor(keys[i], IntArrays4.occurences(
								keys, keys[i]));
				BTreeRange range = btree.searchRange(trans, new Integer(keys[i]));
				BTreeAssert.traverseKeys(range, expectingVisitor);
				expectingVisitor.assertExpectations();
				lastValue = keys[i];
			}
		}
	}
}
