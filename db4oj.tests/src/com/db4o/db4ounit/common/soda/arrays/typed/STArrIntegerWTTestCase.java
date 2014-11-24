/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.arrays.typed;
import com.db4o.query.*;


public class STArrIntegerWTTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public Integer[] intArr;
	
	public STArrIntegerWTTestCase(){
	}
	
	public STArrIntegerWTTestCase(Integer[] arr){
		intArr = arr;
	}
	
	public Object[] createData() {
		return new Object[]{
			new STArrIntegerWTTestCase(),
			new STArrIntegerWTTestCase(new Integer[0]),
			new STArrIntegerWTTestCase(new Integer[] {new Integer(0), new Integer(0)}),
			new STArrIntegerWTTestCase(new Integer[] {new Integer(1), new Integer(17), new Integer(Integer.MAX_VALUE - 1)}),
			new STArrIntegerWTTestCase(new Integer[] {new Integer(3), new Integer(17), new Integer(25), new Integer(Integer.MAX_VALUE - 2)})
		};
	}
	
	public void testDefaultContainsOne(){
		Query q = newQuery();
		
		q.constrain(new STArrIntegerWTTestCase(new Integer[] {new Integer(17)}));
		expect(q, new int[] {3, 4});
	}
	
	public void testDefaultContainsTwo(){
		Query q = newQuery();
		
		q.constrain(new STArrIntegerWTTestCase(new Integer[] {new Integer(17), new Integer(25)}));
		expect(q, new int[] {4});
	}
	
	
	
}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	