/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre12.soda.experiments;
import java.util.*;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.collections.*;




/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class STCurrentTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase {

	public String mystr;

	public STCurrentTestCase() {
	}

	public STCurrentTestCase(String str) {
		this.mystr = str;
	}

	public String toString() {
		return "STCurrentTestCase: " + mystr;
	}

	public Object[] createData() {
		return new Object[] {
			new STVectorEU(new Object[] { new Integer(17)}),
			new STVectorEU(
				new Object[] {
					new Integer(3),
					new Integer(17),
					new Integer(25),
					new Integer(Integer.MAX_VALUE - 2)}),
			new STVectorT(new Object[] { new Integer(17)}),
			new STVectorU(
				new Object[] {
					new Integer(3),
					new Integer(17),
					new Integer(25),
					new Integer(Integer.MAX_VALUE - 2)}),
			};
	}

	// FIXME
	public void _testDescendOne() {
		Query q = newQuery();
		
		q.constrain(STVectorEU.class);
		q.descend("col").constrain(new Integer(17));
		expect(q, new int[] { 0 });
	}

	//	public void testIdentity(){
	//		Query q = SodaTenewQuery();
	//		Constraint c = q.constrain(new STCurrentTestCase("hi"));
	//		ObjectSet set = q.execute();
	//		STCurrentTestCase identityConstraint = (STCurrentTestCase)set.next();
	//		identityConstraint.mystr = "jdjdjd";
	//		q = SodaTenewQuery();
	//		q.constrain(identityConstraint).identity();
	//		identityConstraint.mystr = "hi";
	//		SodaTecom.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q,new STCurrentTestCase("hi"));
	//	}

	//	public void all_Depts_that_have_no_emp(){
	//		Query q = pm.query();
	//		q.constrain(Department.class);
	//		Query qEmps = q.descendant("emps");
	//		qEmps.constrain(null).or(
	//		  qEmps.constrain(new Integer(0)).length()
	//		);
	//		ObjectSet noEmps = q.execute();
	//	}
	//	
	//	public void all_Depts_that_have_exaclty_one_emp(){
	//		Query q = pm.query();
	//		q.constrain(Department.class);
	//		Query qEmps = q.descendant("emps");
	//		qEmps.constrain(new Integer(1)).length();
	//		ObjectSet oneEmp = q.execute();
	//	}
	//	
	//	public void tiger_teams(){
	//		Query q = pm.query();
	//		q.constrain(Department.class);
	//		Query qEmps = q.descendant("emps");
	//		qEmps.constrain(new Integer(2)).length().greater();
	//		qEmps.constrain(new Integer(5)).length().smaller();
	//		qEmps.descendant("salary").constrain(new Float(50000)).greater();
	//		ObjectSet tigerTeams = q.execute();
	//	}

	public static class Employee {
		public String name;
		public Float salary;
		public Department dept;
		public Employee boss;
	}

	public static class Department {
		public String name;
		public Collection emps;
		public Department() {
		}
		public Department(String name) {
		}
	}

}

