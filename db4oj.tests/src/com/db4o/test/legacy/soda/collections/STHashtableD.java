/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.collections;

import java.util.*;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STHashtableD implements STClass {
	
	public static transient SodaTest st;
	
	protected Hashtable vec(Object[] objects){
		Hashtable h = new Hashtable();
		for (int i = 0; i < objects.length; i++) {
			h.put(objects[i], new Integer(i));
		}
		return h;
	}

	public Object[] store() {
		return new Object[] {
			vec(new Object[] { new Integer(5778), new Integer(5779)}), 
			vec(new Object[] { new Integer(5778), new Integer(5789)}),
			vec(new Object[] { "foo577", new STElement("bar577", "barbar577")}),
			vec(new Object[] { "foo5772", new STElement("bar577", "barbar2577")})
		};
	}
	
	public void testDefaultContainsInteger() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(vec(new Object[] { new Integer(5778)}));
		st.expect(q, new Object[] { r[0], r[1] });
	}

	public void testDefaultContainsString() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(vec(new Object[] { "foo577" }));
		st.expect(q, new Object[] { r[2] });
	}

	public void testDefaultContainsTwo() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(vec(new Object[] { new Integer(5778), new Integer(5789)}));
		st.expect(q, new Object[] { r[1] });
	}

	public void testDefaultContainsObject() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(vec(new Object[] { new STElement("bar577", null)}));
		st.expect(q, new Object[] { r[2], r[3] });
	}
	
}