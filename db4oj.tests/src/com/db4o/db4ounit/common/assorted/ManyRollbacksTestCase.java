/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import db4ounit.extensions.*;

public class ManyRollbacksTestCase extends AbstractDb4oTestCase {
	
	private static final int COUNT = 900;
	
	public static class Item {
	}
	
	public void test() throws Exception {
		for (int i = 0; i < COUNT; i++) {
			store(new Item());
			db().rollback();
		}
		reopen();
	}

}
