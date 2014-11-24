/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.arrays.untyped;
import com.db4o.query.*;


public class STArrIntegerUTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public Object[] intArr;
	
	public STArrIntegerUTestCase(){
	}
	
	public STArrIntegerUTestCase(Object[] arr){
		intArr = arr;
	}
	
	public Object[] createData() {
		return new Object[]{
			new STArrIntegerUTestCase(),
			new STArrIntegerUTestCase(new Object[0]),
			new STArrIntegerUTestCase(new Object[] {new Integer(0), new Integer(0)}),
			new STArrIntegerUTestCase(new Object[] {new Integer(1), new Integer(17), new Integer(Integer.MAX_VALUE - 1)}),
			new STArrIntegerUTestCase(new Object[] {new Integer(3), new Integer(17), new Integer(25), new Integer(Integer.MAX_VALUE - 2)})
		};
	}
	
	public void testDefaultContainsOne(){
		Query q = newQuery();
		
		q.constrain(new STArrIntegerUTestCase(new Object[] {new Integer(17)}));
		expect(q, new int[] {3, 4});
	}

	public void testDefaultContainsTwo(){
		Query q = newQuery();
		
		q.constrain(new STArrIntegerUTestCase(new Object[] {new Integer(17), new Integer(25)}));
		expect(q, new int[] {4});
	}
	
	public void testDescendOne(){
		Query q = newQuery();
		
		q.constrain(STArrIntegerUTestCase.class);
		q.descend("intArr").constrain(new Integer(17));
		expect(q, new int[] {3, 4});
	}
	
	public void testDescendTwo(){
		Query q = newQuery();
		
		q.constrain(STArrIntegerUTestCase.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(17));
		qElements.constrain(new Integer(25));
		expect(q, new int[] {4});
	}
	
	public void testDescendSmaller(){
		Query q = newQuery();
		
		q.constrain(STArrIntegerUTestCase.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(3)).smaller();
		expect(q, new int[] {2, 3});
	}
	
	
	
	
	
}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	