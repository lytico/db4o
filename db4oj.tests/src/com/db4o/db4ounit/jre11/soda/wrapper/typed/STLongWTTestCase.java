/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11.soda.wrapper.typed;
import com.db4o.query.*;


public class STLongWTTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public Long i_long;
	
	public STLongWTTestCase(){
	}
	
	private STLongWTTestCase(long a_long){
		i_long = new Long(a_long);
	}
	
	public Object[] createData() {
		return new Object[]{
			new STLongWTTestCase(Long.MIN_VALUE),
			new STLongWTTestCase(- 1),
			new STLongWTTestCase(0),
			new STLongWTTestCase(Long.MAX_VALUE - 1),
		};
	}
	
	public void testEquals(){
		Query q = newQuery();
		q.constrain(new STLongWTTestCase(Long.MIN_VALUE));  
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expect(q, new Object[] {new STLongWTTestCase(Long.MIN_VALUE)});
	}
	
	public void testGreater(){
		Query q = newQuery();
		q.constrain(new STLongWTTestCase(-1));
		q.descend("i_long").constraints().greater();
		
		expect(q, new int[] {2, 3});
	}
	
	public void testSmaller(){
		Query q = newQuery();
		q.constrain(new STLongWTTestCase(1));
		q.descend("i_long").constraints().smaller();
		
		expect(q, new int[] {0, 1, 2});
	}

	public void testBetween() {
		Query q = newQuery();
		q.constrain(new STLongWTTestCase());
		Query sub = q.descend("i_long");
		sub.constrain(new Long(-3)).greater();
		sub.constrain(new Long(3)).smaller();
		
		expect(q, new int[] {1, 2});
	}

	public void testAnd() {
		Query q = newQuery();
		q.constrain(new STLongWTTestCase());
		Query sub = q.descend("i_long");
		sub.constrain(new Long(-3)).greater().and(sub.constrain(new Long(3)).smaller());
		
		expect(q, new int[] {1, 2});
	}

	public void testOr() {
		Query q = newQuery();
		q.constrain(new STLongWTTestCase());
		Query sub = q.descend("i_long");
		sub.constrain(new Long(3)).greater().or(sub.constrain(new Long(-3)).smaller());
		
		expect(q, new int[] {0, 3});
	}

}

