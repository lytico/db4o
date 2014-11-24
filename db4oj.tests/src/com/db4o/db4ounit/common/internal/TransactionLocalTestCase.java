package com.db4o.db4ounit.common.internal;

import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class TransactionLocalTestCase extends AbstractInMemoryDb4oTestCase {
	
	static class Item {
		
		public final Transaction transaction;

		public Item(Transaction transaction) {
			this.transaction = transaction;
		}
	}
	
	private final TransactionLocal<Item> _subject = new TransactionLocal<Item>() {
		@Override
		public Item initialValueFor(Transaction transaction) {
			return new Item(transaction);
		}
	};
	
	private Transaction t1;
	private Transaction t2;
	
	@Override
	protected void db4oSetupAfterStore() throws Exception {
		t1 = newTransaction();
		t2 = newTransaction();
	}
	
	public void testValueRemainsTheSame() {
		Assert.areSame(itemFor(t1), itemFor(t1));
		Assert.areSame(itemFor(t2), itemFor(t2));
	}
	
	public void testDifferentValuesForDifferentTransactions() {
		Assert.areNotSame(itemFor(t1), itemFor(t2));
	}
	
	public void testInitialValueTransaction() {
		Assert.areSame(t1, itemFor(t1).transaction);
		Assert.areSame(t2, itemFor(t2).transaction);
	}
	
	public void testValuesAreDisposedOfOnCommit() {
		final Item itemBeforeCommit = itemFor(t1);
		t1.commit();
		final Item itemAfterCommit = itemFor(t1);
		Assert.areNotSame(itemAfterCommit, itemBeforeCommit);
	}
	
	public void testValuesAreDisposedOfOnRollback() {
		final Item itemBeforeRollback = itemFor(t1);
		t1.rollback();
		final Item itemAfterRollback = itemFor(t1);
		Assert.areNotSame(itemAfterRollback, itemBeforeRollback);
	}

	private Item itemFor(final Transaction transaction) {
	    return transaction.get(_subject).value;
    }

}
