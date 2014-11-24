/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.classes.untypedhierarchy;
// Generierter package-Name

import com.db4o.query.*;



/**
 * epaul:
 * Shows a bug.
 * 
 * carlrosenberger:
 * Fixed!
 * The error was due to the the behaviour of STCompare.java.
 * It compared the syntetic fields in inner classes also.
 * I changed the behaviour to neglect all fields that
 * contain a "$".
 * 
 *
 * @author <a href="mailto:Paul-Ebermann@gmx.de">Paul Ebermann</a>
 * @version 0.1
 */
public class STInnerClassesTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase 
{

	public class Parent
	{
		public Object child;
		public Parent(Object o) { child = o; }
		public String toString() { return "Parent[" + child + "]"; }
		public Parent() {}
	}


	public class Child
	{
		public Object childFirst;
		public Child(Object o ) { childFirst = o; }
		public String toString() { return "Child[" + childFirst + "]"; }
		public Child() {}
	}

	public Object[] createData() {
		return new Object[]
			{
				new Parent(new Child("Example")),
				new Parent(new Child("no Example")),
			};
	}

	/**
	 * Only 
	 */
	public void testNothing()
	{
		Query q = newQuery();
		q.descend("child");
		
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expect(q, _array);
		//SodaTest.log(q);
	}
	
}// STSomeClasses
