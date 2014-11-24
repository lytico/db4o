/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.wrapper.typed;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STFloatWT implements STClass{
	
	public static transient SodaTest st;
	
	Float i_float;
	
	public STFloatWT(){
	}
	
	private STFloatWT(float a_float){
		i_float = new Float(a_float);
	}
	
	public Object[] store() {
		return new Object[]{
			new STFloatWT(Float.MIN_VALUE),
			new STFloatWT((float) 0.0000123),
			new STFloatWT((float) 1.345),
			new STFloatWT(Float.MAX_VALUE),
		};
	}
	
	public void testEquals(){
		Query q = st.query();
		q.constrain(store()[0]); 
		st.expectOne(q, store()[0]);
	}
	
	public void testGreater(){
		Query q = st.query();
		q.constrain(new STFloatWT((float)0.1));
		q.descend("i_float").constraints().greater();
		Object[] r = store();
		st.expect(q, new Object[] { r[2], r[3]});
	}
	
	public void testSmaller(){
		Query q = st.query();
		q.constrain(new STFloatWT((float)1.5));
		q.descend("i_float").constraints().smaller();
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[1], r[2]});
	}
}

