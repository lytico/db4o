/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.arrays.typed;
import com.db4o.query.*;


public class STArrStringTNTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase {

	public String[][][] strArr;

	public STArrStringTNTestCase() {
	}

	public STArrStringTNTestCase(String[][][] arr) {
		strArr = arr;
	}

	public Object[] createData() {
		STArrStringTNTestCase[] arr = new STArrStringTNTestCase[5];
		
		arr[0] = new STArrStringTNTestCase();
		
		String[][][] content = new String[1][1][2];
		arr[1] = new STArrStringTNTestCase(content);
		
		content = new String[1][2][3];
		arr[2] = new STArrStringTNTestCase(content);
		
		content = new String[1][2][3];
		content[0][0][1] = "foo";
		content[0][1][0] = "bar";
		content[0][1][2] = "fly";
		arr[3] = new STArrStringTNTestCase(content);
		
		content = new String[1][2][3];
		content[0][0][0] = "bar";
		content[0][1][0] = "wohay";
		content[0][1][1] = "johy";
		arr[4] = new STArrStringTNTestCase(content);
		
		Object[] ret = new Object[arr.length];
		System.arraycopy(arr, 0, ret, 0, arr.length);
		return ret;
	}

	public void testDefaultContainsOne() {
		Query q = newQuery();
		
		String[][][] content = new String[1][1][1];
		content[0][0][0] = "bar";
		q.constrain(new STArrStringTNTestCase(content));
		expect(q, new int[] { 3, 4 });
	}

	public void testDefaultContainsTwo() {
		Query q = newQuery();
		
		String[][][] content = new String[2][1][1];
		content[0][0][0] = "bar";
		content[1][0][0] = "foo";
		q.constrain(new STArrStringTNTestCase(content));
		expect(q, new int[] { 3 });
	}

	public void testDescendOne() {
		Query q = newQuery();
		
		q.constrain(STArrStringTNTestCase.class);
		q.descend("strArr").constrain("bar");
		expect(q, new int[] { 3, 4 });
	}

	public void testDescendTwo() {
		Query q = newQuery();
		
		q.constrain(STArrStringTNTestCase.class);
		Query qElements = q.descend("strArr");
		qElements.constrain("foo");
		qElements.constrain("bar");
		expect(q, new int[] { 3 });
	}

	public void testDescendOneNot() {
		Query q = newQuery();
		
		q.constrain(STArrStringTNTestCase.class);
		q.descend("strArr").constrain("bar").not();
		expect(q, new int[] { 0, 1, 2 });
	}

	public void testDescendTwoNot() {
		Query q = newQuery();
		
		q.constrain(STArrStringTNTestCase.class);
		Query qElements = q.descend("strArr");
		qElements.constrain("foo").not();
		qElements.constrain("bar").not();
		expect(q, new int[] { 0, 1, 2 });
	}

}
