/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.arrays.typed;
import com.db4o.query.*;


public class STArrStringTTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public String[] strArr;
	
	public STArrStringTTestCase(){
	}
	
	public STArrStringTTestCase(String[] arr){
		strArr = arr;
	}
	
	public Object[] createData() {
		return new Object[]{
			new STArrStringTTestCase(),
			new STArrStringTTestCase(new String[] {null}),
			new STArrStringTTestCase(new String[] {null, null}),
			new STArrStringTTestCase(new String[] {"foo", "bar", "fly"}),
			new STArrStringTTestCase(new String[] {null, "bar", "wohay", "johy"})
		};
	}
	
	public void testDefaultContainsOne(){
		Query q = newQuery();
		
		q.constrain(new STArrStringTTestCase(new String[] {"bar"}));
		expect(q, new int[] {3, 4});
	}
	
	public void testDefaultContainsTwo(){
		Query q = newQuery();
		
		q.constrain(new STArrStringTTestCase(new String[] {"foo", "bar"}));
		expect(q, new int[] {3});
	}
	
	public void testDescendOne(){
		Query q = newQuery();
		
		q.constrain(STArrStringTTestCase.class);
		q.descend("strArr").constrain("bar");
		expect(q, new int[] {3, 4});
	}
	
	public void testDescendTwo(){
		Query q = newQuery();
		
		q.constrain(STArrStringTTestCase.class);
		Query qElements = q.descend("strArr");
		qElements.constrain("foo");
		qElements.constrain("bar");
		expect(q, new int[] {3});
	}
	
	public void testDescendOneNot(){
		Query q = newQuery();
		
		q.constrain(STArrStringTTestCase.class);
		q.descend("strArr").constrain("bar").not();
		expect(q, new int[] {0, 1, 2});
	}
	
	public void testDescendTwoNot(){
		Query q = newQuery();
		
		q.constrain(STArrStringTTestCase.class);
		Query qElements = q.descend("strArr");
		qElements.constrain("foo").not();
		qElements.constrain("bar").not();
		expect(q, new int[] {0, 1, 2});
	}
	
	
}
	
