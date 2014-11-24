/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.btree;

public class BTreeAsSetTestCase extends BTreeTestCaseBase {
	
	/**
	 * For now this won't work completely easy.
	 * If multiple transactions add the same value, there may
	 * be multiple add patches in the BTree with the same value.
	 * 
	 * There could be many of these patches and they could even
	 * be on different nodes, so we may be on the wrong node
	 * when we want to check. 
	 * 
	 * We will have to take a look at this again for unique field
	 * values, so the test can stay here.
	 * 
	 */
	public void _testAddSameValueFromSameTransaction() {
		add(42);
		add(42);
		assertSingleElement(42);
	}

}
