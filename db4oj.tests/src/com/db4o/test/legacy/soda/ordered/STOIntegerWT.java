/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.ordered;


import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STOIntegerWT implements STClass{
	
	public static transient SodaTest st;
	
	Integer i_int;
	
	public STOIntegerWT(){
	}
	
	private STOIntegerWT(int a_int){
		i_int = new Integer(a_int);
	}
	
	public Object[] store() {
		return new Object[]{
			new STOIntegerWT(1001),
			new STOIntegerWT(99),
			new STOIntegerWT(1),
			new STOIntegerWT(909),
			new STOIntegerWT(1001),
			new STOIntegerWT(0),
			new STOIntegerWT(1010),
			new STOIntegerWT()
		};
	}
	
	public void testAscending() {
		Query q = st.query();
		q.constrain(STOIntegerWT.class);
		q.descend("i_int").orderAscending();
		Object[] r = store();
		st.expectOrdered(q, new Object[] { r[7], r[5], r[2],  r[1], r[3], r[0], r[4], r[6],  });
	}
	
	public void testDescending() {
		Query q = st.query();
		q.constrain(STOIntegerWT.class);
		q.descend("i_int").orderDescending();
		Object[] r = store();
		st.expectOrdered(q, new Object[] { r[6], r[4],  r[0], r[3], r[1], r[2], r[5], r[7]  });
	}
	
	public void testAscendingGreater(){
		Query q = st.query();
		q.constrain(STOIntegerWT.class);
		Query qInt = q.descend("i_int");
		qInt.constrain(new Integer(100)).greater();
		qInt.orderAscending();
		Object[] r = store();
		st.expectOrdered(q, new Object[] {r[3], r[0], r[4], r[6]});
	}
}

