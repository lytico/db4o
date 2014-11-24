/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.classes.simple;


import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STBoolean implements STClass1{
	
	public static transient SodaTest st;
	
	public boolean i_boolean;
	
	public STBoolean(){
	}
	
	private STBoolean(boolean a_boolean){
		i_boolean = a_boolean;
	}
	
	public Object[] store() {
		return new Object[]{
			new STBoolean(false),
			new STBoolean(true),
			new STBoolean(false),
			new STBoolean(false)
		};
	}
	
	public void testEqualsTrue(){
		Query q = st.query();
		q.constrain(new STBoolean(true));  
		store();
		st.expectOne(q, new STBoolean(true));
	}
	
	public void testEqualsFalse(){
		Query q = st.query();
		q.constrain(new STBoolean(false));
		q.descend("i_boolean").constrain(new Boolean(false));
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[2], r[3]});
	}
	
	
	
}

