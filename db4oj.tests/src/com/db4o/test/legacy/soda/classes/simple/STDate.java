/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.classes.simple;

import java.util.*;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STDate implements STClass1{
	
	public static transient SodaTest st;
	
	public Date i_date;
	
	public STDate(){
	}
	
	private STDate(Date a_date){
		i_date = a_date;
	}
	
	public Object[] store() {
		return new Object[]{
			new STDate(null),
			new STDate(new Date(4000)),
			new STDate(new Date(5000)),
			new STDate(new Date(6000)),
			new STDate(new Date(7000)),
		};
	}
	
	public void testEquals(){
		Query q = st.query();
		q.constrain(store()[1]); 
		st.expectOne(q, store()[1]);
	}
	
	public void testGreater(){
		Query q = st.query();
		q.constrain(store()[2]);
		q.descend("i_date").constraints().greater();
		Object[] r = store();
		st.expect(q, new Object[] { r[3], r[4]});
	}
	
	public void testSmaller(){
		Query q = st.query();
		q.constrain(store()[4]);
		q.descend("i_date").constraints().smaller();
		Object[] r = store();
		st.expect(q, new Object[] {r[1], r[2], r[3]});
	}
	
	public void testGreaterOrEqual(){
	    Query q = st.query();
	    q.constrain(store()[2]);
	    q.descend("i_date").constraints().greater().equal();
	    Object[] r = store();
	    st.expect(q, new Object[] {r[2], r[3], r[4]});
	    
	}
	
	public void testNotGreaterOrEqual(){
		Query q = st.query();
		q.constrain(store()[3]);
		q.descend("i_date").constraints().not().greater().equal();
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[1], r[2]});
	}
	
	public void testNull(){
		Query q = st.query();
		q.constrain(new STDate());
		q.descend("i_date").constrain(null);
		st.expectOne(q, new STDate(null));
	}
	
	public void testNotNull(){
		Query q = st.query();
		q.constrain(new STDate());
		q.descend("i_date").constrain(null).not();
		Object[] r = store();
		st.expect(q, new Object[] {r[1], r[2], r[3], r[4]});
	}
}

