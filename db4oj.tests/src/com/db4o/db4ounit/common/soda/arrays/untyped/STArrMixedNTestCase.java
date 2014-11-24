/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.arrays.untyped;
import com.db4o.query.*;


public class STArrMixedNTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase {

	public Object[][][] arr;

	public STArrMixedNTestCase() {
	}

	public STArrMixedNTestCase(Object[][][] arr) {
		this.arr = arr;
	}

	public Object[] createData() {
		STArrMixedNTestCase[] arrMixed = new STArrMixedNTestCase[5];
		
		arrMixed[0] = new STArrMixedNTestCase();
		
		Object[][][] content = new Object[1][1][2];
		arrMixed[1] = new STArrMixedNTestCase(content);
		
		content = new Object[2][2][3];
		arrMixed[2] = new STArrMixedNTestCase(content);
		
		content = new Object[2][2][3];
		content[0][0][1] = "foo";
		content[0][1][0] = "bar";
		content[0][1][2] = "fly";
		content[1][0][0] = new Boolean(false);
		arrMixed[3] = new STArrMixedNTestCase(content);
		
		content = new Object[2][2][3];
		content[0][0][0] = "bar";
		content[0][1][0] = "wohay";
		content[0][1][1] = "johy";
		content[1][0][0] = new Integer(12);
		arrMixed[4] = new STArrMixedNTestCase(content);
		
		Object[] ret = new Object[arrMixed.length];
		System.arraycopy(arrMixed, 0, ret, 0, arrMixed.length);
		return ret;
	}

	public void testDefaultContainsString() {
		Query q = newQuery();
		
		Object[][][] content = new Object[1][1][1];
		content[0][0][0] = "bar";
		q.constrain(new STArrMixedNTestCase(content));
		expect(q, new int[] { 3, 4 });
	}
	
	public void testDefaultContainsInteger() {
		Query q = newQuery();
		
		Object[][][] content = new Object[1][1][1];
		content[0][0][0] = new Integer(12);
		q.constrain(new STArrMixedNTestCase(content));
		expect(q, new int[] {  4 });
	}
	
	public void testDefaultContainsBoolean() {
		Query q = newQuery();
		
		Object[][][] content = new Object[1][1][1];
		content[0][0][0] = new Boolean(false);
		q.constrain(new STArrMixedNTestCase(content));
		expect(q, new int[] {  3 });
	}

	public void testDefaultContainsTwo() {
		Query q = newQuery();
		
		Object[][][] content = new Object[2][1][1];
		content[0][0][0] = "bar";
		content[1][0][0] = new Integer(12);
		q.constrain(new STArrMixedNTestCase(content));
		expect(q, new int[] { 4 });
	}

	public void testDescendOne() {
		Query q = newQuery();
		
		q.constrain(STArrMixedNTestCase.class);
		q.descend("arr").constrain("bar");
		expect(q, new int[] { 3, 4 });
	}

	public void testDescendTwo() {
		Query q = newQuery();
		
		q.constrain(STArrMixedNTestCase.class);
		Query qElements = q.descend("arr");
		qElements.constrain("foo");
		qElements.constrain("bar");
		expect(q, new int[] { 3 });
	}

	public void testDescendOneNot() {
		Query q = newQuery();
		
		q.constrain(STArrMixedNTestCase.class);
		q.descend("arr").constrain("bar").not();
		expect(q, new int[] { 0, 1, 2 });
	}

	public void testDescendTwoNot() {
		Query q = newQuery();
		
		q.constrain(STArrMixedNTestCase.class);
		Query qElements = q.descend("arr");
		qElements.constrain("foo").not();
		qElements.constrain("bar").not();
		expect(q, new int[] { 0, 1, 2 });
	}

}
