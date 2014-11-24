/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.wrapper.typed;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STLongWT implements STClass{
	
	public static transient SodaTest st;
	
	Long i_long;
	
	public STLongWT(){
	}
	
	private STLongWT(long a_long){
		i_long = new Long(a_long);
	}
	
	public Object[] store() {
		return new Object[]{
			new STLongWT(Long.MIN_VALUE),
			new STLongWT(- 1),
			new STLongWT(0),
			new STLongWT(Long.MAX_VALUE - 1),
		};
	}
	
	public void testEquals(){
		Query q = st.query();
		q.constrain(new STLongWT(Long.MIN_VALUE));  
		st.expect(q, new Object[] {new STLongWT(Long.MIN_VALUE)});
	}
	
	public void testGreater(){
		Query q = st.query();
		q.constrain(new STLongWT(-1));
		q.descend("i_long").constraints().greater();
		Object[] r = store();
		st.expect(q, new Object[] {r[2], r[3]});
	}
	
	public void testSmaller(){
		Query q = st.query();
		q.constrain(new STLongWT(1));
		q.descend("i_long").constraints().smaller();
		Object[] r = store();
		st.expect(q, new Object[] {r[0], r[1], r[2]});
	}

	public void testBetween() {
		Query q = st.query();
		q.constrain(new STLongWT());
		Query sub = q.descend("i_long");
		sub.constrain(new Long(-3)).greater();
		sub.constrain(new Long(3)).smaller();
		Object[] r = store();
		st.expect(q, new Object[]{r[1], r[2]});
	}

	public void testAnd() {
		Query q = st.query();
		q.constrain(new STLongWT());
		Query sub = q.descend("i_long");
		sub.constrain(new Long(-3)).greater().and(sub.constrain(new Long(3)).smaller());
		Object[] r = store();
		st.expect(q, new Object[]{r[1], r[2]});
	}

	public void testOr() {
		Query q = st.query();
		q.constrain(new STLongWT());
		Query sub = q.descend("i_long");
		sub.constrain(new Long(3)).greater().or(sub.constrain(new Long(-3)).smaller());
		Object[] r = store();
		st.expect(q, new Object[]{r[0], r[3]});
	}

}

