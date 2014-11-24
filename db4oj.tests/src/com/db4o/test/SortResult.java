/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;

public class SortResult {
	private final static int[] ORIG={2,4,1,3};
	
	public int id;
	
	public SortResult() {
		this(0);
	}
	
	public SortResult(int id) {
		this.id = id;
	}

	public int id() {
		return id;
	}

	public String toString() {
		return "<"+id+">";
	}
	
    public void configure(){
        Db4o.configure().optimizeNativeQueries(false);
    }

	public void store() {
		for (int idx = 0; idx < ORIG.length; idx++) {
			SortResult sortResult = new SortResult(ORIG[idx]);
			Test.store(sortResult);
			//System.err.println(sortResult+"/"+Test.objectContainer().ext().getID(sortResult));
		}
	}
	
	public void test() {
		ObjectSet result=Test.objectContainer().queryByExample(SortResult.class);
// FIXME: Why 0 results?
//		ObjectSet result=Test.objectContainer().query(
//				new Predicate() {
//					public boolean match(SortResult candidate) {
//						return true;
//					}
//				});

/*
		result.ext().sort(new QueryComparator() {
			public int compare(Object first, Object second) {
				return ((SortResult)first).id()-((SortResult)second).id();
			}
		});
		Test.ensure(ORIG.length==result.size());
		for(int i=1;i<=ORIG.length;i++) {
			Test.ensure(((SortResult)result.next()).id()==i);
		}
*/
	}
}
