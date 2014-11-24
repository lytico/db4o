/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.wrapper.typed;


import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STDoubleWT implements STClass{
	
	public static transient SodaTest st;
	
	Double i_double;
	
	public STDoubleWT(){
	}
	
	private STDoubleWT(double a_double){
		i_double = new Double(a_double);
	}
	
	public Object[] store() {
		return new Object[]{
			new STDoubleWT(0),
			new STDoubleWT(0),
			new STDoubleWT(1.01),
			new STDoubleWT(99.99),
			new STDoubleWT(909.00)
		};
	}
	
	public void testEquals(){
		Query q = st.query();
		q.constrain(new STDoubleWT(0));  
		
		// Primitive default values are ignored, so we need an 
		// additional constraint:
		q.descend("i_double").constrain(new Double(0));
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[1]});
	}
	
	public void testGreater(){
		Query q = st.query();
		q.constrain(new STDoubleWT(1));
		q.descend("i_double").constraints().greater();
		Object[] r = store();
		st.expect(q, new Object[] {r[2], r[3], r[4]});
	}
	
	public void testSmaller(){
		Query q = st.query();
		q.constrain(new STDoubleWT(1));
		q.descend("i_double").constraints().smaller();
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[1]});
	}
	
	public void testGreaterOrEqual(){
		Query q = st.query();
		q.constrain(store()[2]);
		q.descend("i_double").constraints().greater().equal();
		Object[] r = store();
		st.expect(q, new Object[] {r[2], r[3], r[4]});
	}
	
	public void testGreaterAndNot(){
		Query q = st.query();
		q.constrain(new STDoubleWT());
		Query val = q.descend("i_double");
		val.constrain(new Double(0)).greater();
		val.constrain(new Double(99.99)).not();
		Object[] r = store();
		st.expect(q, new Object[] {r[2], r[4]});
	}
	
}

