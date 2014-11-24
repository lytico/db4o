package com.db4o.db4ounit.common.soda.ordered;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * COR-1188
 */
public class OrderFollowedByConstraintTestCase extends AbstractDb4oTestCase {

	public static class Data {
		public int _id;
		
		public Data(int id) {
			_id = id;
		}
	}

	private final static int[] IDS = { 42, 47, 11, 1, 50, 2 };

	protected void store() throws Exception {
		for (int idIdx = 0; idIdx < IDS.length; idIdx++) {
			store(new Data(IDS[idIdx]));
		}
	}
	
	public void testOrderFollowedByConstraint() {
		Query query = newQuery(Data.class);
		query.descend("_id").orderAscending();
		query.descend("_id").constrain(new Integer(0)).greater();
		assertOrdered(query.execute());
	}

	public void testLastOrderWins() {
		Query query = newQuery(Data.class);
		query.descend("_id").orderDescending();
		query.descend("_id").orderAscending();
		query.descend("_id").constrain(new Integer(0)).greater();
		assertOrdered(query.execute());
	}

	private void assertOrdered(ObjectSet result) {
		Assert.areEqual(IDS.length, result.size());
		int previousId = 0;
		while(result.hasNext()) {
			Data data = (Data)result.next();
			Assert.isTrue(previousId < data._id);
			previousId = data._id;
		}
	}
	
	public static void main(String[] args) {
		new OrderFollowedByConstraintTestCase().runSolo();
	}
	
}
