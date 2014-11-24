/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.arrays.object;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STArrIntegerON implements STClass{
	
	public static transient SodaTest st;
	
	Object intArr;
	
	public STArrIntegerON(){
	}
	
	public STArrIntegerON(int[][][] arr){
		intArr = arr;
	}
	
	public Object[] store() {
		STArrIntegerON[] arr = new STArrIntegerON[5];
		
		arr[0] = new STArrIntegerON();
		
		int[][][] content = new int[0][0][0];
		arr[1] = new STArrIntegerON(content);
		
		content = new int[1][2][3];
		content[0][0][1] = 0;
		content[0][1][0] = 0;
		arr[2] = new STArrIntegerON(content);
		
		content = new int[1][2][3];
		content[0][0][0] = 1;
		content[0][1][0] = 17;
		content[0][1][1] = Integer.MAX_VALUE - 1;
		arr[3] = new STArrIntegerON(content);
		
		content = new int[1][2][2];
		content[0][0][0] = 3;
		content[0][0][1] = 17;
		content[0][1][0] = 25;
		content[0][1][1] = Integer.MAX_VALUE - 2;
		arr[4] = new STArrIntegerON(content);
		
		Object[] ret = new Object[arr.length];
		System.arraycopy(arr, 0, ret, 0, arr.length);
		return ret;
	}
	
	
	public void testDefaultContainsOne(){
		Query q = st.query();
		Object[] r = store();
		int[][][] content = new int[1][1][1];
		content[0][0][0] = 17;
		q.constrain(new STArrIntegerON(content));
		st.expect(q, new Object[] {r[3], r[4]});
	}
	
	public void testDefaultContainsTwo(){
		Query q = st.query();
		Object[] r = store();
		int[][][] content = new int[2][1][1];
		content[0][0][0] = 17;
		content[1][0][0] = 25;
		q.constrain(new STArrIntegerON(content));
		st.expect(q, new Object[] {r[4]});
	}
	
	public void testDescendOne(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrIntegerON.class);
		q.descend("intArr").constrain(new Integer(17));
		st.expect(q, new Object[] {r[3], r[4]});
	}
	
	public void testDescendTwo(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrIntegerON.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(17));
		qElements.constrain(new Integer(25));
		st.expect(q, new Object[] {r[4]});
	}
	
	public void testDescendSmaller(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrIntegerON.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(3)).smaller();
		st.expect(q, new Object[] {r[2], r[3]});
	}
	
	public void testDescendNotSmaller(){
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrIntegerON.class);
		Query qElements = q.descend("intArr");
		qElements.constrain(new Integer(3)).smaller();
		st.expect(q, new Object[] {r[2], r[3]});
	}
	
}
	
