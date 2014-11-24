/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.collections;

import java.util.*;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STVectorD implements STClass {
	
	public static transient SodaTest st;
	
	protected Vector vec(Object[] objects){
		Vector v = new Vector();
		for (int i = 0; i < objects.length; i++) {
			v.add(objects[i]);
		}
		return v;
	}

	public Object[] store() {
		return new Object[] {
			vec(new Object[] { new Integer(7778), new Integer(7779)}), 
			vec(new Object[] { new Integer(7778), new Integer(7789)}),
			vec(new Object[] { "foo777", new STElement("bar777", "barbar777")}),
			vec(new Object[] { "foo7772", new STElement("bar777", "barbar2777")})
		};
	}
	
	public void testDefaultContainsInteger() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(vec(new Object[] { new Integer(7778)}));
		st.expect(q, new Object[] { r[0], r[1] });
	}

	public void testDefaultContainsString() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(vec(new Object[] { "foo777" }));
		st.expect(q, new Object[] { r[2] });
	}

	public void testDefaultContainsTwo() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(vec(new Object[] { new Integer(7778), new Integer(7789)}));
		st.expect(q, new Object[] { r[1] });
	}

	public void testDefaultContainsObject() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(vec(new Object[] { new STElement("bar777", null)}));
		st.expect(q, new Object[] { r[2], r[3] });
	}
	
}