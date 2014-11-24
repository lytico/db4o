/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.soda.collections;
import java.util.*;

import com.db4o.query.*;



/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class STVectorDTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase {
	
	protected Vector vec(Object[] objects){
		Vector v = new Vector();
		for (int i = 0; i < objects.length; i++) {
			v.add(objects[i]);
		}
		return v;
	}

	public Object[] createData() {
		return new Object[] {
			vec(new Object[] { new Integer(7778), new Integer(7779)}), 
			vec(new Object[] { new Integer(7778), new Integer(7789)}),
			vec(new Object[] { "foo777", new STElement("bar777", "barbar777")}),
			vec(new Object[] { "foo7772", new STElement("bar777", "barbar2777")})
		};
	}
	
	public void testDefaultContainsInteger() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { new Integer(7778)}));
		expect(q, new int[] { 0, 1 });
	}

	public void testDefaultContainsString() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { "foo777" }));
		expect(q, new int[] { 2 });
	}

	public void testDefaultContainsTwo() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { new Integer(7778), new Integer(7789)}));
		expect(q, new int[] { 1 });
	}

	public void testDefaultContainsObject() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { new STElement("bar777", null)}));
		expect(q, new int[] { 2, 3 });
	}
	
}