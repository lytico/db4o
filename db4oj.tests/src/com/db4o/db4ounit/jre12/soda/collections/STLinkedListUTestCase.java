/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.soda.collections;
import java.util.*;

import com.db4o.query.*;



/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class STLinkedListUTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase {

	Object col;

	public STLinkedListUTestCase() {
	}

	public STLinkedListUTestCase(Object[] arr) {
		col = new LinkedList();
		for (int i = 0; i < arr.length; i++) {
			((LinkedList)col).add(arr[i]);
		}
	}

	public Object[] createData() {
		return new Object[] {
			new STLinkedListUTestCase(),
			new STLinkedListUTestCase(new Object[0]),
			new STLinkedListUTestCase(new Object[] { new Integer(0), new Integer(0)}),
			new STLinkedListUTestCase(
				new Object[] {
					new Integer(1),
					new Integer(17),
					new Integer(Integer.MAX_VALUE - 1)}),
			new STLinkedListUTestCase(
				new Object[] {
					new Integer(3),
					new Integer(17),
					new Integer(25),
					new Integer(Integer.MAX_VALUE - 2)}),
			new STLinkedListUTestCase(new Object[] { "foo", new STElement("bar", "barbar")}),
			new STLinkedListUTestCase(new Object[] { "foo2", new STElement("bar", "barbar2")})
		};
	}

	public void testDefaultContainsInteger() {
		Query q = newQuery();
		
		q.constrain(new STLinkedListUTestCase(new Object[] { new Integer(17)}));
		expect(q, new int[] { 3, 4 });
	}

	public void testDefaultContainsString() {
		Query q = newQuery();
		
		q.constrain(new STLinkedListUTestCase(new Object[] { "foo" }));
		expect(q, new int[] { 5 });
	}

	public void testDefaultContainsTwo() {
		Query q = newQuery();
		
		q.constrain(new STLinkedListUTestCase(new Object[] { new Integer(17), new Integer(25)}));
		expect(q, new int[] { 4 });
	}

	public void testDescendOne() {
		Query q = newQuery();
		
		q.constrain(STLinkedListUTestCase.class);
		q.descend("col").constrain(new Integer(17));
		expect(q, new int[] { 3, 4 });
	}

	public void testDescendTwo() {
		Query q = newQuery();
		
		q.constrain(STLinkedListUTestCase.class);
		Query qElements = q.descend("col");
		qElements.constrain(new Integer(17));
		qElements.constrain(new Integer(25));
		expect(q, new int[] { 4 });
	}

	public void testDescendSmaller() {
		Query q = newQuery();
		
		q.constrain(STLinkedListUTestCase.class);
		Query qElements = q.descend("col");
		qElements.constrain(new Integer(3)).smaller();
		expect(q, new int[] { 2, 3 });
	}
	
	public void testDefaultContainsObject() {
		Query q = newQuery();
		
		q.constrain(new STLinkedListUTestCase(new Object[] { new STElement("bar", null)}));
		expect(q, new int[] { 5, 6 });
	}
	
	public void testDescendToObject() {
		Query q = newQuery();
		
		q.constrain(new STLinkedListUTestCase());
		q.descend("col").descend("foo1").constrain("bar");
		expect(q, new int[] { 5, 6 });
	}

}