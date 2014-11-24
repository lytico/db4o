/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections;

import com.db4o.*;
import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CascadeDeleteSubClassTestCase extends AbstractDb4oTestCase {
	
	public static class Member{
		
		public String _name;
		
		public Member(String name){
			_name = name;
		}
		
	}
	
	public static class SuperClass{
		
		public Member _superClassMember; 
		
	}
	
	public static class SubClass extends SuperClass{
		
		public Member _subClassMember;
		
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.objectClass(SubClass.class).cascadeOnDelete(true);
	}
	
	@Override
	protected void store() throws Exception {
		SubClass subClass = new SubClass();
		subClass._superClassMember = new Member("_superClassMember");
		subClass._subClassMember = new Member("_subClassMember");
		store(subClass);
	}
	
	public void test(){
		SubClass subClass = retrieveOnlyInstance(SubClass.class);
		db().delete(subClass);
		db().commit();
		ObjectSet objectSet = db().query(Member.class);
		Assert.areEqual(0, objectSet.size());
	}

}
