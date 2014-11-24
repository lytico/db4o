/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.wrapper.untyped;


import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STBooleanWU implements STClass1{
	
	final static String DESCENDANT = "i_boolean";

	public static transient SodaTest st;
	
	public Object i_boolean;
	
	public STBooleanWU(){
	}
	
	private STBooleanWU(boolean a_boolean){
		i_boolean = new Boolean(a_boolean);
	}
	
	public Object[] store() {
		return new Object[]{
			new STBooleanWU(false),
			new STBooleanWU(true),
			new STBooleanWU(false),
			new STBooleanWU(false),
			new STBooleanWU()
		};
	}
	
	public void testEqualsTrue(){
		Query q = st.query();
		q.constrain(new STBooleanWU(true));  
		Object[] r = store();
		st.expectOne(q, new STBooleanWU(true));
	}
	
	public void testEqualsFalse(){
		Query q = st.query();
		q.constrain(new STBooleanWU(false));
		q.descend(DESCENDANT).constrain(new Boolean(false));
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[2], r[3]});
	}
	
	public void testNull(){
		Query q = st.query();
		q.constrain(new STBooleanWU());
		q.descend(DESCENDANT).constrain(null);
		Object[] r = store();
		st.expectOne(q, new STBooleanWU());
	}
	
	public void testNullOrTrue(){
		Query q = st.query();
		q.constrain(new STBooleanWU());
		Query qd = q.descend(DESCENDANT);
		qd.constrain(null).or(qd.constrain(new Boolean(true)));
		Object[] r = store();
		st.expect(q, new Object[] {r[1], r[4]});
	}
	
	public void testNotNullAndFalse(){
		Query q = st.query();
		q.constrain(new STBooleanWU());
		Query qd = q.descend(DESCENDANT);
		qd.constrain(null).not().and(qd.constrain(new Boolean(false)));
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[2], r[3]});
	}
	
}

