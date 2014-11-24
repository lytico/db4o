/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.classes.typedhierarchy;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

/** SDFT: Same descendant field typed*/
public class STSDFT1 implements STClass{
	
	public static transient SodaTest st;
	
	public STSDFT1(){
	}
	
	public Object[] store() {
		return new Object[]{
			new STSDFT1(),
			new STSDFT2(),
			new STSDFT2("str1"),
			new STSDFT2("str2"),
			new STSDFT3(),
			new STSDFT3("str1"),
			new STSDFT3("str3")
		};
	}
	
	public void testStrNull(){
		Query q = st.query();
		q.constrain(new STSDFT1());
		q.descend("foo").constrain(null);
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[1], r[4]});
	}
	
	public void testStrVal(){
		Query q = st.query();
		q.constrain(STSDFT1.class);
		q.descend("foo").constrain("str1");
		Object[] r = store();
		st.expect(q, new Object[] {r[2], r[5]});
	}
	
	public void testOrValue(){
		Query q = st.query();
		q.constrain(STSDFT1.class);
		Query foo = q.descend("foo");
		foo.constrain("str1").or(foo.constrain("str2"));
		Object[] r = store();
		st.expect(q, new Object[] {r[2], r[3], r[5]});
	}
	
	public void testOrNull(){
		Query q = st.query();
		q.constrain(STSDFT1.class);
		Query foo = q.descend("foo");
		foo.constrain("str1").or(foo.constrain(null));
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[1], r[2], r[4], r[5]});
	}
	
	public void testTripleOrNull(){
		Query q = st.query();
		q.constrain(STSDFT1.class);
		Query foo = q.descend("foo");
		foo.constrain("str1").or(foo.constrain(null)).or(foo.constrain("str2"));
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[1], r[2],r[3], r[4], r[5]});
	}

// work in progress
	
//	public void testOverConstrainedByClass(){
//		Query q = SodaTest.query();
//		q.constrain(STSDFT1.class).or(q.constrain(STSDFT2.class));
//		Query foo = q.descend("foo");
//		foo.constrain("str1").or(foo.constrain(null)).or(foo.constrain("str2"));
//		Object[] r = store();
//		SodaTest.expect(q, new Object[] {r[0], r[1], r[2],r[3], r[4], r[5]});
//	}
	
}

