/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class SortedSameOrder {
	private static class Sortable {
		private String a;

		public Sortable(String a) {
			this.a = a;
		}
		
		public String toString() {
			return a;
		}
	}

	private static class SortableComparator implements QueryComparator {
		public int compare(Object first, Object second) {
			return ((Sortable)first).a.compareTo(((Sortable)second).a);
		}
	}

	public void store() {
		Test.store(new Sortable("a"));
		Test.store(new Sortable("c"));
		Test.store(new Sortable("b"));
	}
	
	public void test() {
		Query query=Test.query();
		query.constrain(Sortable.class);
		SortableComparator cmp = new SortableComparator();
		query.sortBy(cmp);
		ObjectSet result=query.execute();
		
		Object last=null;
		while(result.hasNext()) {
			Object cur=result.next();
			Test.ensure(last==null||cmp.compare(last,cur)<=0);
			last=cur;
		}
		last=null;
		for (int i=0;i<result.size();i++) {
			Object cur = result.get(i);
			Test.ensure(last==null||cmp.compare(last,cur)<=0);
			last=cur;
		}
	}
}
