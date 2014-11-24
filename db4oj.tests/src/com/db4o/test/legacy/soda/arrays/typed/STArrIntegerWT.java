/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.arrays.typed;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STArrIntegerWT implements STClass{
	
	public static transient SodaTest st;
	
	Integer[] intArr;
	
	public STArrIntegerWT(){
	}
	
	public STArrIntegerWT(Integer[] arr){
		intArr = arr;
	}
	
	public Object[] store() {
		return new Object[]{
			new STArrIntegerWT(),
			new STArrIntegerWT(new Integer[0]),
			new STArrIntegerWT(new Integer[] {new Integer(0), new Integer(0)}),
			new STArrIntegerWT(new Integer[] {new Integer(1), new Integer(17), new Integer(Integer.MAX_VALUE - 1)}),
			new STArrIntegerWT(new Integer[] {new Integer(3), new Integer(17), new Integer(25), new Integer(Integer.MAX_VALUE - 2)})
		};
	}
	
	public void testDefaultContainsOne(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STArrIntegerWT(new Integer[] {new Integer(17)}));
		st.expect(q, new Object[] {r[3], r[4]});
	}
	
	public void testDefaultContainsTwo(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STArrIntegerWT(new Integer[] {new Integer(17), new Integer(25)}));
		st.expect(q, new Object[] {r[4]});
	}
	
	
	
}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	