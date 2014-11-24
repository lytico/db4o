/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.classes.simple;
import com.db4o.query.*;


public class STFloatTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public float i_float;
	
	public STFloatTestCase(){
	}
	
	private STFloatTestCase(float a_float){
		i_float = a_float;
	}
	
	public Object[] createData() {
		return new Object[]{
			new STFloatTestCase(Float.MIN_VALUE),
			new STFloatTestCase((float) 0.0000123),
			new STFloatTestCase((float) 1.345),
			new STFloatTestCase(Float.MAX_VALUE),
		};
	}
	
	public void testEquals(){
		Query q = newQuery();
		q.constrain(_array[0]); 
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, _array[0]);
	}
	
	public void testGreater(){
		Query q = newQuery();
		q.constrain(new STFloatTestCase((float)0.1));
		q.descend("i_float").constraints().greater();
		
		expect(q, new int[] { 2, 3});
	}
	
	public void testSmaller(){
		Query q = newQuery();
		q.constrain(new STFloatTestCase((float)1.5));
		q.descend("i_float").constraints().smaller();
		
		expect(q, new int[] {0, 1, 2});
	}
}

