/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.arrays.untyped;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STArrIntegerU implements STClass{
	
	public static transient SodaTest st;
	
	Object[] intArr;
	
	public STArrIntegerU(){
	}
	
	public STArrIntegerU(Object[] arr){
		intArr = arr;
	}
	
	public Object[] store() {
		return new Object[]{
			new STArrIntegerU(),
			new STArrIntegerU(new Object[0]),
			new STArrIntegerU(new Object[] {new Integer(0), new Integer(0)}),
			new STArrIntegerU(new Object[] {new Integer(1), new Integer(17), new Integer(Integer.MAX_VALUE - 1)}),
			new STArrIntegerU(new Object[] {new Integer(3), new Integer(17), new Integer(25), new Integer(Integer.MAX_VALUE - 2)})
		};
	}
	
	public void testDefaultContainsOne(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STArrIntegerU(new Object[] {new Integer(17)}));
		st.expect(q, new Object[] {r[3], r[4]});
	}

	public void testDefaultContainsTwo(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(new STArrIntegerU(new Object[] {new Integer(17), new Integer(25)}));
		st.expect(q, new Object[] {r[4]});
	}
	
	public void testDescendOne(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrIntegerU.class);
		q.descend("intArr").constrain(new Integer(17));
		st.expect(q, new Object[] {r[3], r[4]});
	}
	
	public void testDescendTwo(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrIntegerU.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(17));
		qElements.constrain(new Integer(25));
		st.expect(q, new Object[] {r[4]});
	}
	
	public void testDescendSmaller(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrIntegerU.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(3)).smaller();
		st.expect(q, new Object[] {r[2], r[3]});
	}
	
	
	
	
	
}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	