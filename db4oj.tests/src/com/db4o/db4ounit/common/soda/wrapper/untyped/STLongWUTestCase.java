/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.wrapper.untyped;
import com.db4o.query.*;


public class STLongWUTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public Object i_long;
	
	public STLongWUTestCase(){
	}
	
	private STLongWUTestCase(long a_long){
		i_long = new Long(a_long);
	}
	
	public Object[] createData() {
		return new Object[]{
			new STLongWUTestCase(Long.MIN_VALUE),
			new STLongWUTestCase(- 1),
			new STLongWUTestCase(0),
			new STLongWUTestCase(Long.MAX_VALUE - 1),
		};
	}
	
	public void testEquals(){
		Query q = newQuery();
		q.constrain(new STLongWUTestCase(Long.MIN_VALUE));  
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expect(q, new Object[] {new STLongWUTestCase(Long.MIN_VALUE)});
	}
	
	public void testGreater(){
		Query q = newQuery();
		q.constrain(new STLongWUTestCase(-1));
		q.descend("i_long").constraints().greater();
		
		expect(q, new int[] {2, 3});
	}
	
	public void testSmaller(){
		Query q = newQuery();
		q.constrain(new STLongWUTestCase(1));
		q.descend("i_long").constraints().smaller();
		
		expect(q, new int[] {0, 1, 2});
	}

	public void testBetween() {
		Query q = newQuery();
		q.constrain(new STLongWUTestCase());
		Query sub = q.descend("i_long");
		sub.constrain(new Long(-3)).greater();
		sub.constrain(new Long(3)).smaller();
		
		expect(q, new int[] {1, 2});
	}

	public void testAnd() {
		Query q = newQuery();
		q.constrain(new STLongWUTestCase());
		Query sub = q.descend("i_long");
		sub.constrain(new Long(-3)).greater().and(sub.constrain(new Long(3)).smaller());
		
		expect(q, new int[] {1, 2});
	}

	public void testOr() {
		Query q = newQuery();
		q.constrain(new STLongWUTestCase());
		Query sub = q.descend("i_long");
		sub.constrain(new Long(3)).greater().or(sub.constrain(new Long(-3)).smaller());
		
		expect(q, new int[] {0, 3});
	}

}

