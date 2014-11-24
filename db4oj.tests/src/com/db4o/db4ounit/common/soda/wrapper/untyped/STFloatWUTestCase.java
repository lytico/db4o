/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.wrapper.untyped;
import com.db4o.query.*;


public class STFloatWUTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public Object i_float;
	
	public STFloatWUTestCase(){
	}
	
	private STFloatWUTestCase(float a_float){
		i_float = new Float(a_float);
	}
	
	public Object[] createData() {
		return new Object[]{
			new STFloatWUTestCase(Float.MIN_VALUE),
			new STFloatWUTestCase((float) 0.0000123),
			new STFloatWUTestCase((float) 1.345),
			new STFloatWUTestCase(Float.MAX_VALUE),
		};
	}
	
	public void testEquals(){
		Query q = newQuery();
		q.constrain(_array[0]); 
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, _array[0]);
	}
	
	public void testGreater(){
		Query q = newQuery();
		q.constrain(new STFloatWUTestCase((float)0.1));
		q.descend("i_float").constraints().greater();
		
		expect(q, new int[] { 2, 3});
	}
	
	public void testSmaller(){
		Query q = newQuery();
		q.constrain(new STFloatWUTestCase((float)1.5));
		q.descend("i_float").constraints().smaller();
		
		expect(q, new int[] {0, 1, 2});
	}
}

