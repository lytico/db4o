/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.arrays.object;
import com.db4o.query.*;


public class STArrIntegerWTONTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public Object intArr;
	
	public STArrIntegerWTONTestCase(){
	}
	
	public STArrIntegerWTONTestCase(Integer[][][] arr){
		intArr = arr;
	}
	
	public Object[] createData() {
		STArrIntegerWTONTestCase[] arr = new STArrIntegerWTONTestCase[5];
		
		arr[0] = new STArrIntegerWTONTestCase();
		
		Integer[][][] content = new Integer[0][0][0];
		arr[1] = new STArrIntegerWTONTestCase(content);
		
		content = new Integer[1][2][3];
		content[0][0][1] = new Integer(0);
		content[0][1][0] = new Integer(0);
		arr[2] = new STArrIntegerWTONTestCase(content);
		
		content = new Integer[1][2][3];
		content[0][0][0] = new Integer(1);
		content[0][1][0] = new Integer(17);
		content[0][1][1] = new Integer(Integer.MAX_VALUE - 1);
		arr[3] = new STArrIntegerWTONTestCase(content);
		
		content = new Integer[1][2][2];
		content[0][0][0] = new Integer(3);
		content[0][0][1] = new Integer(17);
		content[0][1][0] = new Integer(25);
		content[0][1][1] = new Integer(Integer.MAX_VALUE - 2);
		arr[4] = new STArrIntegerWTONTestCase(content);
		
		Object[] ret = new Object[arr.length];
		System.arraycopy(arr, 0, ret, 0, arr.length);
		return ret;
	}
	
	
	public void testDefaultContainsOne(){
		Query q = newQuery();
		
		Integer[][][] content = new Integer[1][1][1];
		content[0][0][0] = new Integer(17);
		q.constrain(new STArrIntegerWTONTestCase(content));
		expect(q, new int[] {3, 4});
	}
	
	public void testDefaultContainsTwo(){
		Query q = newQuery();
		
		Integer[][][] content = new Integer[2][1][1];
		content[0][0][0] = new Integer(17);
		content[1][0][0] = new Integer(25);
		q.constrain(new STArrIntegerWTONTestCase(content));
		expect(q, new int[] {4});
	}
	
	public void testDescendOne(){
		Query q = newQuery();
		
		q.constrain(STArrIntegerWTONTestCase.class);
		q.descend("intArr").constrain(new Integer(17));
		expect(q, new int[] {3, 4});
	}
	
	public void testDescendTwo(){
		Query q = newQuery();
		
		q.constrain(STArrIntegerWTONTestCase.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(17));
		qElements.constrain(new Integer(25));
		expect(q, new int[] {4});
	}
	
	public void testDescendSmaller(){
		Query q = newQuery();
		
		q.constrain(STArrIntegerWTONTestCase.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(3)).smaller();
		expect(q, new int[] {2, 3});
	}
	
	public void testDescendNotSmaller(){
		Query q = newQuery();
		
		q.constrain(STArrIntegerWTONTestCase.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(3)).smaller();
		expect(q, new int[] {2, 3});
	}
	
}
	
