/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.collections;

import java.util.*;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STHashtableU implements STClass {
	
	public static transient SodaTest st;
	
	Object col;

	public STHashtableU() {
	}

	public STHashtableU(Object[] arr) {
		Hashtable ht = new Hashtable();
		for (int i = 0; i < arr.length; i++) {
			ht.put(arr[i], new Integer(i));
		}
		col = ht;
	}

	public Object[] store() {
		return new Object[] {
			new STHashtableU(),
				new STHashtableU(new Object[0]),
				new STHashtableU(new Object[] { new Integer(0), new Integer(0)}),
				new STHashtableU(
					new Object[] {
						new Integer(1),
						new Integer(17),
						new Integer(Integer.MAX_VALUE - 1)}),
				new STHashtableU(
					new Object[] {
						new Integer(3),
						new Integer(17),
						new Integer(25),
						new Integer(Integer.MAX_VALUE - 2)}),
				new STHashtableU(new Object[] { "foo", new STElement("bar", "barbar")}),
				new STHashtableU(new Object[] { "foo2", new STElement("bar", "barbar2")})
		};
	}
	
	public void testDefaultContainsInteger() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STHashtableU(new Object[] { new Integer(17)}));
		st.expect(q, new Object[] { r[3], r[4] });
	}

	public void testDefaultContainsString() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STHashtableU(new Object[] { "foo" }));
		st.expect(q, new Object[] { r[5] });
	}

	public void testDefaultContainsTwo() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STHashtableU(new Object[] { new Integer(17), new Integer(25)}));
		st.expect(q, new Object[] { r[4] });
	}

	public void testDescendOne() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(STHashtableU.class);
		q.descend("col").constrain(new Integer(17));
		st.expect(q, new Object[] { r[3], r[4] });
	}

	public void testDescendTwo() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(STHashtableU.class);
		Query qElements = q.descend("col");
		qElements.constrain(new Integer(17));
		qElements.constrain(new Integer(25));
		st.expect(q, new Object[] { r[4] });
	}

	public void testDescendSmaller() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(STHashtableU.class);
		Query qElements = q.descend("col");
		qElements.constrain(new Integer(3)).smaller();
		st.expect(q, new Object[] { r[2], r[3] });
	}
	
	public void testDefaultContainsObject() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STHashtableU(new Object[] { new STElement("bar", null)}));
		st.expect(q, new Object[] { r[5], r[6] });
	}
	
	public void testDescendToObject() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STHashtableU());
		q.descend("col").descend("foo1").constrain("bar");
		st.expect(q, new Object[] { r[5], r[6] });
	}

}