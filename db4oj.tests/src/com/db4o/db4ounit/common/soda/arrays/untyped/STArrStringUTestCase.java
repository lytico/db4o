/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.arrays.untyped;
import com.db4o.query.*;


public class STArrStringUTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public Object[] strArr;
	
	public STArrStringUTestCase(){
	}
	
	public STArrStringUTestCase(Object[] arr){
		strArr = arr;
	}
	
	public Object[] createData() {
		return new Object[]{
			new STArrStringUTestCase(),
			new STArrStringUTestCase(new Object[] {null}),
			new STArrStringUTestCase(new Object[] {null, null}),
			new STArrStringUTestCase(new Object[] {"foo", "bar", "fly"}),
			new STArrStringUTestCase(new Object[] {null, "bar", "wohay", "johy"})
		};
	}

	public void testDefaultContainsOne(){
		Query q = newQuery();
		
		q.constrain(new STArrStringUTestCase(new Object[] {"bar"}));
		expect(q, new int[] {3, 4});
	}

	public void testDefaultContainsTwo(){
		Query q = newQuery();
		
		q.constrain(new STArrStringUTestCase(new Object[] {"foo", "bar"}));
		expect(q, new int[] {3});
	}
	
	public void testDescendOne(){
		Query q = newQuery();
		
		q.constrain(STArrStringUTestCase.class);
		q.descend("strArr").constrain("bar");
		expect(q, new int[] {3, 4});
	}
	
	public void testDescendTwo(){
		Query q = newQuery();
		
		q.constrain(STArrStringUTestCase.class);
		Query qElements = q.descend("strArr");
		qElements.constrain("foo");
		qElements.constrain("bar");
		expect(q, new int[] {3});
	}
	
	public void testDescendOneNot(){
		Query q = newQuery();
		
		q.constrain(STArrStringUTestCase.class);
		q.descend("strArr").constrain("bar").not();
		expect(q, new int[] {0, 1, 2});
	}
	
	public void testDescendTwoNot(){
		Query q = newQuery();
		
		q.constrain(STArrStringUTestCase.class);
		Query qElements = q.descend("strArr");
		qElements.constrain("foo").not();
		qElements.constrain("bar").not();
		expect(q, new int[] {0, 1, 2});
	}
	
}