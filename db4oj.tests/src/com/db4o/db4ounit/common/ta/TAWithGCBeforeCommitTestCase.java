package com.db4o.db4ounit.common.ta;

import com.db4o.activation.*;
import com.db4o.config.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class TAWithGCBeforeCommitTestCase extends AbstractDb4oTestCase {

	private static final String UPDATED_ID = "X";
	private static final String ORIG_ID = "U";

	public static class Item implements Activatable {

		public String _id;
		
		public Item(String id) {
			_id = id;
		}
		
		public void id(String id) {
			activate(ActivationPurpose.WRITE);
			_id = id;
		}
		
		public String id() {
			activate(ActivationPurpose.READ);
			return _id;
		}
		
		private transient Activator _activator;

		public void bind(Activator activator) {
			if (this._activator == activator) {
				return;
			}
			if (activator != null && this._activator != null) {
				throw new IllegalStateException();
			}
			this._activator = activator;
		}

		public void activate(ActivationPurpose purpose) {
			if (this._activator == null) return;
			this._activator.activate(purpose);
		}

	}

	@Override
	protected void configure(Configuration config) throws Exception {
		config.add(new TransparentPersistenceSupport());
	}
	
	@Override
	protected void store() throws Exception {
		store(new Item(ORIG_ID));
	}
	
	public void testCommit() {
		Item item = (Item)retrieveOnlyInstance(Item.class);
		item.id(UPDATED_ID);
		item = null;
		System.gc();
		db().commit();
		item = (Item)retrieveOnlyInstance(Item.class);
		db().refresh(item, Integer.MAX_VALUE);
		Assert.areEqual(UPDATED_ID, item.id());
	}

	public void testRollback() {
		Item item = (Item)retrieveOnlyInstance(Item.class);
		item.id(UPDATED_ID);
		item = null;
		System.gc();
		db().rollback();
		item = (Item)retrieveOnlyInstance(Item.class);
		db().refresh(item, Integer.MAX_VALUE);
		Assert.areEqual(ORIG_ID, item.id());
	}

	public static void main(String[] args) {
		new TAWithGCBeforeCommitTestCase().runAll();
	}
}
