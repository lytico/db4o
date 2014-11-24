/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.arrays.object;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STArrIntegerO implements STClass{
	
	public static transient SodaTest st;
	
	Object intArr;
	
	public STArrIntegerO(){
	}
	
	public STArrIntegerO(Object[] arr){
		intArr = arr;
	}
	
	public Object[] store() {
		return new Object[]{
			new STArrIntegerO(),
			new STArrIntegerO(new Object[0]),
			new STArrIntegerO(new Object[] {new Integer(0), new Integer(0)}),
			new STArrIntegerO(new Object[] {new Integer(1), new Integer(17), new Integer(Integer.MAX_VALUE - 1)}),
			new STArrIntegerO(new Object[] {new Integer(3), new Integer(17), new Integer(25), new Integer(Integer.MAX_VALUE - 2)})
		};
	}
	
	public void testDefaultContainsOne(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STArrIntegerO(new Object[] {new Integer(17)}));
		st.expect(q, new Object[] {r[3], r[4]});
	}

	public void testDefaultContainsTwo(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STArrIntegerO(new Object[] {new Integer(17), new Integer(25)}));
		st.expect(q, new Object[] {r[4]});
	}
	
	public void testDescendOne(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrIntegerO.class);
		q.descend("intArr").constrain(new Integer(17));
		st.expect(q, new Object[] {r[3], r[4]});
	}
	
	public void testDescendTwo(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrIntegerO.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(17));
		qElements.constrain(new Integer(25));
		st.expect(q, new Object[] {r[4]});
	}
	
	public void testDescendSmaller(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrIntegerO.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(3)).smaller();
		st.expect(q, new Object[] {r[2], r[3]});
	}
	
	
	
	
	
}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	