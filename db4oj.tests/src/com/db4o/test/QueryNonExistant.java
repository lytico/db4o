/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.query.*;

public class QueryNonExistant {
	
	QueryNonExistant1 member;
	
	public QueryNonExistant(){
		// db4o constructor
	}
	
	public QueryNonExistant(boolean createMembers){
		member = new QueryNonExistant1();
		member.member = new QueryNonExistant2();
		member.member.member = this;
		// db4o constructor
	}
	
	public void test(){
		ObjectContainer con = Test.objectContainer(); 
		con.queryByExample((new QueryNonExistant(true)));
		Test.ensureOccurrences(new QueryNonExistant(), 0);
		Query q = con.query();
		q.constrain(new QueryNonExistant(true));
		Test.ensure(q.execute().size() == 0);
	}
	
	public static class QueryNonExistant1{
		QueryNonExistant2 member;
	}
	
	public static class QueryNonExistant2 extends QueryNonExistant1{
		QueryNonExistant member;
	}
	
}
