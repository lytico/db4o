/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.classes.untypedhierarchy;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

/** RUH: RoundTrip Untyped Hierarchy */
public class STRUH1 implements STClass{
	
	public static transient SodaTest st;
	
	Object h2;
	String foo1;
	
	public STRUH1(){
	}
	
	public STRUH1(STRUH2 a2){
		h2 = a2;
	}
	
	public STRUH1(String str){
		foo1 = str;
	}
	
	public STRUH1(STRUH2 a2, String str){
		h2 = a2;
		foo1 = str;
	}
	
	public Object[] store() {
		
		STRUH1[] objects = {
			new STRUH1(),
			new STRUH1("str1"),
			new STRUH1(new STRUH2()),
			new STRUH1(new STRUH2("str2")),
			new STRUH1(new STRUH2(new STRUH3("str3"))),
			new STRUH1(new STRUH2(new STRUH3("str3"), "str2"))
		};
		for (int i = 0; i < objects.length; i++) {
			objects[i].adjustParents();
			
		}
		return objects;
	}
	
	/** this is the special part of this test: circular references */
	void adjustParents(){
		if(h2 != null){
			STRUH2 th2 = (STRUH2)h2;
			
			th2.parent = this;
			if(th2.h3 != null){
				STRUH3 th3 = (STRUH3)th2.h3;
				th3.parent = th2;
				th3.grandParent = this;
			}
		}
	}
	
	public void testStrNull(){
		Query q = st.query();
		q.constrain(new STRUH1());
		q.descend("foo1").constrain(null);
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[2], r[3], r[4], r[5]});
	}

	public void testBothNull(){
		Query q = st.query();
		q.constrain(new STRUH1());
		q.descend("foo1").constrain(null);
		q.descend("h2").constrain(null);
		st.expectOne(q, store()[0]);
	}

	public void testDescendantNotNull(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STRUH1());
		q.descend("h2").constrain(null).not();
		st.expect(q, new Object[] {r[2], r[3], r[4], r[5]});
	}
	
	public void testDescendantDescendantNotNull(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STRUH1());
		q.descend("h2").descend("h3").constrain(null).not();
		st.expect(q, new Object[] {r[4], r[5]});
	}
	
	public void testDescendantExists(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(r[2]);
		st.expect(q, new Object[] {r[2], r[3], r[4], r[5]});
	}
	
	public void testDescendantValue(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(r[3]);
		st.expect(q, new Object[] {r[3], r[5]});
	}
	
	public void testDescendantDescendantExists(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STRUH1(new STRUH2(new STRUH3())));
		st.expect(q, new Object[] {r[4], r[5]});
	}
	
	public void testDescendantDescendantValue(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STRUH1(new STRUH2(new STRUH3("str3"))));
		st.expect(q, new Object[] {r[4], r[5]});
	}
	
	public void testDescendantDescendantStringPath(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STRUH1());
		q.descend("h2").descend("h3").descend("foo3").constrain("str3");
		st.expect(q, new Object[] {r[4], r[5]});
	}
	
	public void testSequentialAddition(){
		Query q = st.query();
		store();
		q.constrain(new STRUH1());
		Query cur = q.descend("h2");
		cur.constrain(new STRUH2());
		cur.descend("foo2").constrain("str2");
		cur = cur.descend("h3");
		cur.constrain(new STRUH3());
		cur.descend("foo3").constrain("str3");
		st.expectOne(q, store()[5]);
	}
	
	public void testTwoLevelOr(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STRUH1("str1"));
		q.descend("foo1").constraints().or(
			q.descend("h2").descend("h3").descend("foo3").constrain("str3")
		);
		st.expect(q, new Object[] {r[1], r[4], r[5]});
	}
	
	public void testThreeLevelOr(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STRUH1("str1"));
		q.descend("foo1").constraints().or(
			q.descend("h2").descend("foo2").constrain("str2")
		).or(
			q.descend("h2").descend("h3").descend("foo3").constrain("str3")
		);
		st.expect(q, new Object[] {r[1], r[3], r[4], r[5]});
	}
}

