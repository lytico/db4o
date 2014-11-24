/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.soda.collections;
import java.util.*;

import com.db4o.query.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class STHashtableEDTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase {
	
	public static class ExtendHashtable extends Hashtable{
	}
	
	protected ExtendHashtable vec(Object[] objects){
		ExtendHashtable h = new ExtendHashtable();
		for (int i = 0; i < objects.length; i++) {
			h.put(objects[i], new Integer(i));
		}
		return h;
	}

	public Object[] createData() {
		return new Object[] {
			vec(new Object[] { new Integer(6778), new Integer(6779)}), 
			vec(new Object[] { new Integer(6778), new Integer(6789)}),
			vec(new Object[] { "foo677", new STElement("bar677", "barbar677")}),
			vec(new Object[] { "foo6772", new STElement("bar677", "barbar2677")})
		};
	}
	
	public void testDefaultContainsInteger() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { new Integer(6778)}));
		expect(q, new int[] { 0, 1 });
	}

	public void testDefaultContainsString() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { "foo677" }));
		expect(q, new int[] { 2 });
	}

	public void testDefaultContainsTwo() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { new Integer(6778), new Integer(6789)}));
		expect(q, new int[] { 1 });
	}

	public void testDefaultContainsObject() {
		Query q = newQuery();
		
		q.constrain(vec(new Object[] { new STElement("bar677", null)}));
		expect(q, new int[] { 2, 3 });
	}
	
}