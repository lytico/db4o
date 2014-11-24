/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.experiments;

import java.util.*;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;
import com.db4o.test.legacy.soda.collections.*;


/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class STCurrent implements STClass {

	public static transient SodaTest st;
	
	SodaTest pm;

	String mystr;

	public STCurrent() {
	}

	public STCurrent(String str) {
		this.mystr = str;
	}

	public String toString() {
		return "STCurrent: " + mystr;
	}

	public Object[] store() {
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

	public void testDescendOne() {
		Query q = st.query();
		Object[] r = store();
		q.constrain(STVectorEU.class);
		q.descend("col").constrain(new Integer(17));
		st.expect(q, new Object[] { r[0] });
	}

	//	public void testIdentity(){
	//		Query q = SodaTest.query();
	//		Constraint c = q.constrain(new STCurrent("hi"));
	//		ObjectSet set = q.execute();
	//		STCurrent identityConstraint = (STCurrent)set.next();
	//		identityConstraint.mystr = "jdjdjd";
	//		q = SodaTest.query();
	//		q.constrain(identityConstraint).identity();
	//		identityConstraint.mystr = "hi";
	//		SodaTest.expectOne(q,new STCurrent("hi"));
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

}

class Employee {
	String name;
	Float salary;
	Department dept;
	Employee boss;
}

class Department {
	String name;
	Collection emps;
	Department() {
	}
	Department(String name) {
	}
}
