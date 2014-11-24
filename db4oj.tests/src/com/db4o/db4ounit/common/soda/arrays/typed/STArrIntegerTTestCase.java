/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.arrays.typed;
import com.db4o.query.*;


public class STArrIntegerTTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public int[] intArr;
	
	public static void main(String[] args) {
		new STArrIntegerTTestCase().runSolo();
	}
	
	public STArrIntegerTTestCase(){
	}
	
	public STArrIntegerTTestCase(int[] arr){
		intArr = arr;
	}
	
	public Object[] createData() {
		return new Object[]{
			new STArrIntegerTTestCase(),
			new STArrIntegerTTestCase(new int[0]),
			new STArrIntegerTTestCase(new int[] {0, 0}),
			new STArrIntegerTTestCase(new int[] {1, 17, Integer.MAX_VALUE - 1}),
			new STArrIntegerTTestCase(new int[] {3, 17, 25, Integer.MAX_VALUE - 2})
		};
	}
	
	public void _testDefaultContainsOne(){
		Query q = newQuery();
		
		q.constrain(new STArrIntegerTTestCase(new int[] {17}));
		expect(q, new int[] {3, 4});
	}
	
	public void _testDefaultContainsTwo(){
		Query q = newQuery();
		
		q.constrain(new STArrIntegerTTestCase(new int[] {17, 25}));
		expect(q, new int[] {4});
	}
	
	public void testDescendOne(){
		Query q = newQuery();
		
		q.constrain(STArrIntegerTTestCase.class);
		q.descend("intArr").constrain(new Integer(17));
		expect(q, new int[] {3, 4});
	}
	
	public void testDescendTwo(){
		Query q = newQuery();
		
		q.constrain(STArrIntegerTTestCase.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(17));
		qElements.constrain(new Integer(25));
		expect(q, new int[] {4});
	}
	
	public void testDescendSmaller(){
		Query q = newQuery();
		
		q.constrain(STArrIntegerTTestCase.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(3)).smaller();
		expect(q, new int[] {2, 3});
	}
	
	public void testDescendNotSmaller(){
		Query q = newQuery();
		
		q.constrain(STArrIntegerTTestCase.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(3)).smaller();
		expect(q, new int[] {2, 3});
	}
	
	
	
}
	
