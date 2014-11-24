package com.db4o.db4ounit.common.cs;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class QueryConsistencyTestCase extends AbstractDb4oTestCase implements OptOutAllButNetworkingCS {
	
	public static void main(String[] args) {
	    new Db4oTestSuite() {

			@Override
            protected Class[] testCases() {
				return new Class[] { QueryConsistencyTestCase.class };
            }
	    	
	    }.runAll();
    }
	

	public static class Item {
		public int _id;

		public Item(int id) {
			_id = id;
		}
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.optimizeNativeQueries(false);
	}
	
	@Override
	protected void store() throws Exception {
		store(new Item(42));
	}
	
	public void testDelete() {		
		final Item found = sodaQueryForItem(42);
		Assert.areEqual(42, found._id);
		
		db().delete(found);
		
		Assert.isNull(sodaQueryForItem(42));
		Assert.isNull(nativeQueryForItem(42));
		
		db().commit();
		Assert.isNull(sodaQueryForItem(42));
		Assert.isNull(nativeQueryForItem(42));
	}
	
	public void testUpdate() {		
		final Item found = sodaQueryForItem(42);
		Assert.areEqual(42, found._id);
		Assert.areSame(found, nativeQueryForItem(42));
		
		found._id = 21;
		
		Assert.isNull(sodaQueryForItem(21));
		Assert.areSame(found, sodaQueryForItem(42));
		Assert.areSame(found, nativeQueryForItem(42));
		
		store(found);
		
		Assert.areSame(found, sodaQueryForItem(21));
		Assert.areEqual(21, found._id);
		Assert.areSame(found, nativeQueryForItem(21));
		Assert.areEqual(21, found._id);
		
		db().commit();
		Assert.areSame(found, nativeQueryForItem(21));
	}
	
	private Item nativeQueryForItem(final int id) {
		final ObjectSet<Item> result = db().query(new ItemById(id));
		return firstOrNull(result);
	}

	public static final class ItemById extends Predicate<Item> {
		public int _id;
		
		public ItemById(int id) {
			_id = id;
		}
		
		@Override
		public boolean match(Item candidate) {
			return candidate._id == _id;
		}
	}
	
	private Item sodaQueryForItem(final int id) {
	    final Query q = db().query();
	    q.constrain(Item.class);
	    q.descend("_id").constrain(id).equal();
	    return firstOrNull(q.<Item>execute());
    }

	private <T> T firstOrNull(final ObjectSet<T> result) {
		return result.hasNext() ? result.next() : null;
    }

}
