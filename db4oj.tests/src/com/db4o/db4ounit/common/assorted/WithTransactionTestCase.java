/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class WithTransactionTestCase extends AbstractDb4oTestCase {
	
	public void test(){
		final Transaction originalTransaction = container().transaction();
		Transaction transaction = null;
		synchronized(container().lock()){
			transaction = container().newUserTransaction();
		}
		final Transaction finalTransaction = transaction; 
		container().withTransaction(transaction, new Runnable() {
			public void run() {
				Assert.areSame(finalTransaction, container().transaction());
			}
		});
		Assert.areSame(originalTransaction, container().transaction());
	}

}
