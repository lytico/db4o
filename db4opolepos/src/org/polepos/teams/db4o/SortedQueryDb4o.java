package org.polepos.teams.db4o;

import org.polepos.circuits.sortedquery.*;
import org.polepos.runner.db4o.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

public class SortedQueryDb4o extends Db4oDriver implements SortedQueryDriver {
	

	@Override
	public void configure(Configuration config) {
		
	}

	public void query_ascending() {
		Query query = db().query();
		query.constrain(Item.class);
		query.sortBy(new AscendingIdComparator());
		query.execute();
	}

	public void query_descending() {
		Query query = db().query();
		query.constrain(Item.class);
		query.sortBy(new DescendingIdComparator());
		query.execute();
	}

	public void store() {
		int count = setup().getObjectCount();
		begin();
		for (int i = 1; i <= count; i++) {
			store(new Item(i));
		}
		commit();
	}

	public static class AscendingIdComparator implements QueryComparator<Item> {
		public int compare(Item first, Item second) {
			return first.id - second.id;
		}
	}

	public static class DescendingIdComparator implements QueryComparator<Item> {
		public int compare(Item first, Item second) {
			return second.id - first.id;
		}
	}


}
