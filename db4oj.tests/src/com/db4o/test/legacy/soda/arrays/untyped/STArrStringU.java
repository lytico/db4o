/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.arrays.untyped;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STArrStringU implements STClass{
	
	public static transient SodaTest st;
	
	Object[] strArr;
	
	public STArrStringU(){
	}
	
	public STArrStringU(Object[] arr){
		strArr = arr;
	}
	
	public Object[] store() {
		return new Object[]{
			new STArrStringU(),
			new STArrStringU(new Object[] {null}),
			new STArrStringU(new Object[] {null, null}),
			new STArrStringU(new Object[] {"foo", "bar", "fly"}),
			new STArrStringU(new Object[] {null, "bar", "wohay", "johy"})
		};
	}

	public void testDefaultContainsOne(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STArrStringU(new Object[] {"bar"}));
		st.expect(q, new Object[] {r[3], r[4]});
	}

	public void testDefaultContainsTwo(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STArrStringU(new Object[] {"foo", "bar"}));
		st.expect(q, new Object[] {r[3]});
	}
	
	public void testDescendOne(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrStringU.class);
		q.descend("strArr").constrain("bar");
		st.expect(q, new Object[] {r[3], r[4]});
	}
	
	public void testDescendTwo(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrStringU.class);
		Query qElements = q.descend("strArr");
		qElements.constrain("foo");
		qElements.constrain("bar");
		st.expect(q, new Object[] {r[3]});
	}
	
	public void testDescendOneNot(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrStringU.class);
		q.descend("strArr").constrain("bar").not();
		st.expect(q, new Object[] {r[0], r[1], r[2]});
	}
	
	public void testDescendTwoNot(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrStringU.class);
		Query qElements = q.descend("strArr");
		qElements.constrain("foo").not();
		qElements.constrain("bar").not();
		st.expect(q, new Object[] {r[0], r[1], r[2]});
	}
	
}