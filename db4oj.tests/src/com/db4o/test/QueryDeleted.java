/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

public class QueryDeleted {
	
	public String name;
	
	public QueryDeleted(){
	}
	
	public QueryDeleted(String name){
		this.name = name;
	}
	
	public void store(){
		Test.deleteAllInstances(this);
		Test.store(new QueryDeleted("one"));
		Test.store(new QueryDeleted("two"));
	}
	
	public void test(){
		Query q = Test.query();
		q.constrain(QueryDeleted.class);
		q.descend("name").constrain("one");
		QueryDeleted qd = (QueryDeleted)q.execute().next();
		Test.delete(qd);
		checkCount(1);
		Test.rollBack();
		checkCount(2);
		Test.delete(qd);
		checkCount(1);
		Test.commit();
		checkCount(1);
	}
	
	private void checkCount(int count){
	    Query q = Test.query();
	    q.constrain(QueryDeleted.class);
	    ObjectSet res = q.execute();
	    Test.ensure(res.size() == count);
	}
	
	
}
