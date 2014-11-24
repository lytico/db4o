/* Copyright (C) 2006  db4objects Inc.  http://www.db4o.com */

package demo.objectmanager.model;

import com.db4o.ObjectContainer;

import java.util.Date;


/**
 * @exclude
 */
public class Inheritance {
	
	
	public static class Inheritance0 {
		
		public String s0;
		
	}
	
	public static class Inheritance1 extends Inheritance0 {
		
		public String s1;
		
	}
	
	public static class Inheritance2 extends Inheritance1 {
		
		public String s2;
		
	}

	public static class InheritanceB1 {
		public String str;
	}

	public static class InheritanceB2 extends InheritanceB1 {
		public int i;
	}

	public static class InheritanceB3 extends InheritanceB2 {
		public Date date;
	}

	public static void forDemo(ObjectContainer db){
		Inheritance2 inheritance2 = new Inheritance2();
		inheritance2.s0 = "inheritance2-0";
		inheritance2.s1 = "inheritance2-1";
		inheritance2.s2 = "inheritance2-2";
		db.set(inheritance2);

		Inheritance1 inheritance1 = new Inheritance1();
		inheritance1.s0 = "inheritance1-0";
		inheritance1.s1 = "inheritance1-1";
		db.set(inheritance1);

		Inheritance0 inheritance0 = new Inheritance0();
		inheritance0.s0 = "inheritance0-0";
		db.set(inheritance0);


		// the order of inserts does matter when querying on subclasses, that's why this second set is here
		InheritanceB1 l_t1 = new InheritanceB1();
		l_t1.str = "InheritanceB1";
		db.set(l_t1);
		InheritanceB2 l_t2 = new InheritanceB2();
		l_t2.str = "InheritanceB2";
		l_t2.i = 222;
		db.set(l_t2);
		InheritanceB3 l_t3 = new InheritanceB3();
		l_t3.str = "InheritanceB3";
		l_t3.i = 333;
		l_t3.date = new Date();
		db.set(l_t3);

	}

}
