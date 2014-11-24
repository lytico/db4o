/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.classes.untypedhierarchy;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;
import com.db4o.test.legacy.soda.classes.typedhierarchy.*;


/** UH: Untyped Hierarchy */
public class STUH1 implements STClass {

	public static transient SodaTest st;
	
	Object h2;
	Object foo1;

	public STUH1() {
	}

	public STUH1(STUH2 a2) {
		h2 = a2;
	}

	public STUH1(String str) {
		foo1 = str;
	}

	public STUH1(STUH2 a2, String str) {
		h2 = a2;
		foo1 = str;
	}

	public Object[] store() {
		return new Object[] {
			new STUH1(),
			new STUH1("str1"),
			new STUH1(new STUH2()),
			new STUH1(new STUH2("str2")),
			new STUH1(new STUH2(new STUH3("str3"))),
			new STUH1(new STUH2(new STUH3("str3"), "str2")),
			};
	}

	public void testStrNull() {
		Query q = st.query();
		q.constrain(new STUH1());
		q.descend("foo1").constrain(null);
		Object[] r = store();
		st.expect(q, new Object[] { r[0], r[2], r[3], r[4], r[5] });
	}

	public void testBothNull() {
		Query q = st.query();
		q.constrain(new STUH1());
		q.descend("foo1").constrain(null);
		q.descend("h2").constrain(null);
		st.expectOne(q, store()[0]);
	}

	public void testDescendantNotNull() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STUH1());
		q.descend("h2").constrain(null).not();
		st.expect(q, new Object[] { r[2], r[3], r[4], r[5] });
	}

	public void testDescendantDescendantNotNull() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STUH1());
		q.descend("h2").descend("h3").constrain(null).not();
		st.expect(q, new Object[] { r[4], r[5] });
	}

	public void testDescendantExists() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(r[2]);
		st.expect(q, new Object[] { r[2], r[3], r[4], r[5] });
	}

	public void testDescendantValue() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(r[3]);
		st.expect(q, new Object[] { r[3], r[5] });
	}

	public void testDescendantDescendantExists() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STUH1(new STUH2(new STUH3())));
		st.expect(q, new Object[] { r[4], r[5] });
	}

	public void testDescendantDescendantValue() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STUH1(new STUH2(new STUH3("str3"))));
		st.expect(q, new Object[] { r[4], r[5] });
	}

	public void testDescendantDescendantStringPath() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STUH1());
		q.descend("h2").descend("h3").descend("foo3").constrain("str3");
		st.expect(q, new Object[] { r[4], r[5] });
	}

	public void testSequentialAddition() {
		Query q = st.query();
		q.constrain(new STUH1());
		store();
		Query cur = q.descend("h2");
		cur.constrain(new STUH2());
		cur.descend("foo2").constrain("str2");
		cur = cur.descend("h3");
		cur.constrain(new STUH3());
		cur.descend("foo3").constrain("str3");
		st.expectOne(q, store()[5]);
	}

	public void testTwoLevelOr() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STUH1("str1"));
		q.descend("foo1").constraints().or(
			q.descend("h2").descend("h3").descend("foo3").constrain("str3"));
		st.expect(q, new Object[] { r[1], r[4], r[5] });
	}

	public void testThreeLevelOr() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STUH1("str1"));
		q.descend("foo1").constraints().or(
			q.descend("h2").descend("foo2").constrain("str2")).or(
			q.descend("h2").descend("h3").descend("foo3").constrain("str3"));

		st.expect(q, new Object[] { r[1], r[3], r[4], r[5] });
	}

	public void testNonExistentDescendant() {
		Query q = st.query();
		STUH1 constraint = new STUH1();
		constraint.foo1 = new STETH2();
		q.constrain(constraint);
		st.expectNone(q);
	}

}
