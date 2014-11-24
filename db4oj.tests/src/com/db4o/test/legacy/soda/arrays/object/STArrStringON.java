/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.arrays.object;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STArrStringON implements STClass {

	public static transient SodaTest st;
	
	Object strArr;

	public STArrStringON() {
	}

	public STArrStringON(Object[][][] arr) {
		strArr = arr;
	}

	public Object[] store() {
		STArrStringON[] arr = new STArrStringON[5];
		
		arr[0] = new STArrStringON();
		
		String[][][] content = new String[1][1][2];
		arr[1] = new STArrStringON(content);
		
		content = new String[1][2][3];
		arr[2] = new STArrStringON(content);
		
		content = new String[1][2][3];
		content[0][0][1] = "foo";
		content[0][1][0] = "bar";
		content[0][1][2] = "fly";
		arr[3] = new STArrStringON(content);
		
		content = new String[1][2][3];
		content[0][0][0] = "bar";
		content[0][1][0] = "wohay";
		content[0][1][1] = "johy";
		arr[4] = new STArrStringON(content);
		
		Object[] ret = new Object[arr.length];
		System.arraycopy(arr, 0, ret, 0, arr.length);
		return ret;
	}

	public void testDefaultContainsOne() {
		Query q = st.query();
		Object[] r = store();
		String[][][] content = new String[1][1][1];
		content[0][0][0] = "bar";
		q.constrain(new STArrStringON(content));
		st.expect(q, new Object[] { r[3], r[4] });
	}

	public void testDefaultContainsTwo() {
		Query q = st.query();
		Object[] r = store();
		String[][][] content = new String[2][1][1];
		content[0][0][0] = "bar";
		content[1][0][0] = "foo";
		q.constrain(new STArrStringON(content));
		st.expect(q, new Object[] { r[3] });
	}

	public void testDescendOne() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrStringON.class);
		q.descend("strArr").constrain("bar");
		st.expect(q, new Object[] { r[3], r[4] });
	}

	public void testDescendTwo() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrStringON.class);
		Query qElements = q.descend("strArr");
		qElements.constrain("foo");
		qElements.constrain("bar");
		st.expect(q, new Object[] { r[3] });
	}

	public void testDescendOneNot() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrStringON.class);
		q.descend("strArr").constrain("bar").not();
		st.expect(q, new Object[] { r[0], r[1], r[2] });
	}

	public void testDescendTwoNot() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(STArrStringON.class);
		Query qElements = q.descend("strArr");
		qElements.constrain("foo").not();
		qElements.constrain("bar").not();
		st.expect(q, new Object[] { r[0], r[1], r[2] });
	}

}
