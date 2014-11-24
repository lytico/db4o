/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.ordered;


import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STOInteger implements STClass{
	
	public static transient SodaTest st;
	
	int i_int;
	
	public STOInteger(){
	}
	
	private STOInteger(int a_int){
		i_int = a_int;
	}
	
	public String toString(){
		return "STInteger: " + i_int;
	}
	
	public Object[] store() {
		return new Object[]{
			new STOInteger(1001),
			new STOInteger(99),
			new STOInteger(1),
			new STOInteger(909),
			new STOInteger(1001),
			new STOInteger(0),
			new STOInteger(1010),
		};
	}
	
	public void testAscending() {
		Query q = st.query();
		q.constrain(STOInteger.class);
		q.descend("i_int").orderAscending();
		Object[] r = store();
		st.expectOrdered(q, new Object[] { r[5], r[2],  r[1], r[3], r[0], r[4], r[6] });
	}
	
	public void testDescending() {
		Query q = st.query();
		q.constrain(STOInteger.class);
		q.descend("i_int").orderDescending();
		Object[] r = store();
		st.expectOrdered(q, new Object[] { r[6], r[4],  r[0], r[3], r[1], r[2], r[5] });
	}
	
	public void testAscendingGreater(){
		Query q = st.query();
		q.constrain(STOInteger.class);
		Query qInt = q.descend("i_int");
		qInt.constrain(new Integer(100)).greater();
		qInt.orderAscending();
		Object[] r = store();
		st.expectOrdered(q, new Object[] {r[3], r[0], r[4], r[6] });
	}
	
}

