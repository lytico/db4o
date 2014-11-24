/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.classes.typedhierarchy;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

/** ETH: Extends Typed Hierarchy */
public class STETH1 implements STClass{
	
	public static transient SodaTest st;
	
	String foo1;
	
	public STETH1(){
	}
	
	public STETH1(String str){
		foo1 = str;
	}
	
	public Object[] store() {
		return new Object[]{
			new STETH1(),
			new STETH1("str1"),
			new STETH2(),
			new STETH2("str1", "str2"),
			new STETH3(),
			new STETH3("str1a", "str2", "str3"),
			new STETH3("str1a", "str2a", null),
			new STETH4(),
			new STETH4("str1a", "str2", "str4"),
			new STETH4("str1b", "str2a", "str4")
		};
	}
	
	public void testStrNull(){
		Query q = st.query();
		q.constrain(new STETH1());
		q.descend("foo1").constrain(null);
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[2], r[4], r[7]});
	}

	public void testTwoNull(){
		Query q = st.query();
		q.constrain(new STETH1());
		q.descend("foo1").constrain(null);
		q.descend("foo3").constrain(null);
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[2], r[4], r[7]});
	}
	
	public void testClass(){
		Query q = st.query();
		q.constrain(STETH2.class);
		Object[] r = store();
		st.expect(q, new Object[] {r[2], r[3], r[4], r[5], r[6], r[7], r[8], r[9]});
	}
	
	public void testOrClass(){
		Query q = st.query();
		q.constrain(STETH3.class).or(q.constrain(STETH4.class));
		Object[] r = store();
		st.expect(q, new Object[] {r[4], r[5], r[6], r[7], r[8], r[9]});
	}
	
	public void testAndClass(){
		Query q = st.query();
		q.constrain(STETH1.class);
		q.constrain(STETH4.class);
		Object[] r = store();
		st.expect(q, new Object[] {r[7], r[8], r[9]});
	}
	
	public void testParalellDescendantPaths(){
		Query q = st.query();
		q.constrain(STETH3.class).or(
		q.constrain(STETH4.class));
		q.descend("foo3").constrain("str3").or(
		q.descend("foo4").constrain("str4"));
		Object[] r = store();
		st.expect(q, new Object[] {r[5], r[8], r[9]});
	}
	
	public void testOrObjects(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(r[3]).or(q.constrain(r[5]));
		st.expect(q, new Object[] {r[3], r[5]});
	}
	
}

