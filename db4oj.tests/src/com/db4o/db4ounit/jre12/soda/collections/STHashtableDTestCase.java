/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.soda.collections;
import java.util.*;

import com.db4o.query.*;



/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class STHashtableDTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase {
	
	protected Hashtable vec(Object[] objects){
		Hashtable h = new Hashtable();
		for (int i = 0; i < objects.length; i++) {
			h.put(objects[i], new Integer(i));
		}
		return h;
	}

	public Object[] createData() {
		return new Object[] {
			vec(new Object[] { new Integer(5778), new Integer(5779)}), 
			vec(new Object[] { new Integer(5778), new Integer(5789)}),
			vec(new Object[] { "foo577", new STElement("bar577", "barbar577")}),
			vec(new Object[] { "foo5772", new STElement("bar577", "barbar2577")})
		};
	}
	
	public void testDefaultContainsInteger() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { new Integer(5778)}));
		expect(q, new int[] { 0, 1 });
	}

	public void testDefaultContainsString() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { "foo577" }));
		expect(q, new int[] { 2 });
	}

	public void testDefaultContainsTwo() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { new Integer(5778), new Integer(5789)}));
		expect(q, new int[] { 1 });
	}

	public void testDefaultContainsObject() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { new STElement("bar577", null)}));
		expect(q, new int[] { 2, 3 });
	}
	
}