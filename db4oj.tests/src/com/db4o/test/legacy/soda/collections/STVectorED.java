/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.collections;

import java.util.*;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STVectorED implements STClass {
	
	public static transient SodaTest st;
	
	public static class ExtendVector extends Vector{
	}
	
	protected ExtendVector vec(Object[] objects){
		ExtendVector v = new ExtendVector();
		for (int i = 0; i < objects.length; i++) {
			v.add(objects[i]);
		}
		return v;
	}

	public Object[] store() {
		return new Object[] {
			vec(new Object[] { new Integer(8778), new Integer(8779)}), 
			vec(new Object[] { new Integer(8778), new Integer(8789)}),
			vec(new Object[] { "foo877", new STElement("bar877", "barbar877")}),
			vec(new Object[] { "foo8772", new STElement("bar877", "barbar2877")})
		};
	}
	
	public void testDefaultContainsInteger() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(vec(new Object[] { new Integer(8778)}));
		st.expect(q, new Object[] { r[0], r[1] });
	}

	public void testDefaultContainsString() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(vec(new Object[] { "foo877" }));
		st.expect(q, new Object[] { r[2] });
	}

	public void testDefaultContainsTwo() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(vec(new Object[] { new Integer(8778), new Integer(8789)}));
		st.expect(q, new Object[] { r[1] });
	}

	public void testDefaultContainsObject() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(vec(new Object[] { new STElement("bar877", null)}));
		st.expect(q, new Object[] { r[2], r[3] });
	}
	
}