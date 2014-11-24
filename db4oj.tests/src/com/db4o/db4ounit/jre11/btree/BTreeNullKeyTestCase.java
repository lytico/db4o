/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.jre11.btree;

import com.db4o.db4ounit.common.btree.*;
import com.db4o.foundation.*;

import db4ounit.*;

public class BTreeNullKeyTestCase extends BTreeTestCaseBase {	

	public void testKeysCantBeNull() {
		final Integer value = null;
		Assert.expect(ArgumentNullException.class, new CodeBlock() {
			public void run() throws Throwable {
				add(value);
			}
		});
	}

	private void add(Object element) {
		_btree.add(trans(), element);
	}
	
	public static void main(String[] args) {
		new BTreeNullKeyTestCase().runSolo();
	}
}
