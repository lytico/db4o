/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.arrays.untyped;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STArrMixed implements STClass{
	
	public static transient SodaTest st;
	
	Object[] arr;
	
	public STArrMixed(){
	}
	
	public STArrMixed(Object[] arr){
		this.arr = arr;
	}
	
	public Object[] store() {
		return new Object[]{
			new STArrMixed(),
			new STArrMixed(new Object[0]),
			new STArrMixed(new Object[] {new Integer(0), new Integer(0), "foo", new Boolean(false)}),
			new STArrMixed(new Object[] {new Integer(1), new Integer(17), new Integer(Integer.MAX_VALUE - 1), "foo", "bar"}),
			new STArrMixed(new Object[] {new Integer(3), new Integer(17), new Integer(25), new Integer(Integer.MAX_VALUE - 2)})
		};
	}
	
	public void testDefaultContainsInteger(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STArrMixed(new Object[] {new Integer(17)}));
		st.expect(q, new Object[] {r[3], r[4]});
	}
	
	public void testDefaultContainsString(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STArrMixed(new Object[] {"foo"}));
		st.expect(q, new Object[] {r[2], r[3]});
	}
	
	public void testDefaultContainsBoolean(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STArrMixed(new Object[] {new Boolean(false)}));
		st.expect(q, new Object[] {r[2]});
	}

	public void testDefaultContainsTwo(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STArrMixed(new Object[] {new Integer(17), "bar"}));
		st.expect(q, new Object[] {r[3]});
	}
	
	public void testDescendOne(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrMixed.class);
		q.descend("arr").constrain(new Integer(17));
		st.expect(q, new Object[] {r[3], r[4]});
	}
	
	public void testDescendTwo(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrMixed.class);
		Query qElements = q.descend("arr");
		qElements.constrain(new Integer(17));
		qElements.constrain("bar");
		st.expect(q, new Object[] {r[3]});
	}
	
	public void testDescendSmaller(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrMixed.class);
		Query qElements = q.descend("arr");
		qElements.constrain(new Integer(3)).smaller();
		st.expect(q, new Object[] {r[2], r[3]});
	}
	
}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	