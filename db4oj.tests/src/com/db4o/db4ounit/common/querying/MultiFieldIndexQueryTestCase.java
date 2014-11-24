/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.querying;


import java.lang.reflect.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @exclude
 */
public class MultiFieldIndexQueryTestCase extends AbstractDb4oTestCase {
	
	public static void main(String[] args) {
		new MultiFieldIndexQueryTestCase().runSolo();
	}
	
	public static class Book {
	    
	    public Person[] authors;
	    public String title;

	    public Book(){}

	    public Book(String title, Person[] authors){
	        this.title = title;
	        this.authors = authors;
	    }

	    public String toString(){
	        String ret = title;
	        if(authors != null){
	            for (int i = 0; i < authors.length; i++) {
	                ret += "\n  " + authors[i].toString(); 
	            }
	        }
	        return ret;
	    }
	}
	
	public static class Person {
	    
	    public String firstName;
	    public String lastName;

	    public Person(){}

	    public Person(String firstName, String lastName){
	        this.firstName = firstName;
	        this.lastName = lastName;
	    }

	    public String toString(){
	        return "Person " + firstName + " " + lastName;
	    }
	}
	
	protected void configure(Configuration config) {
		indexAllFields(config,Book.class);
		indexAllFields(config,Person.class);		
	}
	
	protected void indexAllFields(Configuration config,Class clazz) {
		final Field[] fields = clazz.getDeclaredFields();
		for (int i = 0; i < fields.length; i++) {
			indexField(config,clazz, fields[i].getName());
		}
		final Class superclass = clazz.getSuperclass();
		if (superclass != null) {
			indexAllFields(config,superclass);
		}
	}

	protected void store() throws Exception {
		Person aaron = new Person("Aaron", "OneOK");
		Person bill = new Person("Bill", "TwoOK");
		Person chris = new Person("Chris", "ThreeOK");
		Person dave = new Person("Dave", "FourOK");
		Person neil = new Person("Neil", "Notwanted");
		Person nat = new Person("Nat", "Neverwanted");
		db().store(new Book("Persistence possibilities", new Person[] { aaron,
				bill, chris }));
		db().store(new Book("Persistence using S.O.D.A.",
				new Person[] { aaron }));
		db().store(new Book("Persistence using JDO",
				new Person[] { bill, dave }));
		db().store(new Book("Don't want to find Phil", new Person[] { aaron,
				bill, neil }));
		db().store(new Book("Persistence by Jeff", new Person[] { nat }));
	}

	public void test() {
		Query qBooks = newQuery();
		qBooks.constrain(Book.class);
		qBooks.descend("title").constrain("Persistence").like();
		Query qAuthors = qBooks.descend("authors");
		Query qFirstName = qAuthors.descend("firstName");
		Query qLastName = qAuthors.descend("lastName");
		Constraint cAaron = qFirstName.constrain("Aaron").and(
				qLastName.constrain("OneOK"));
		Constraint cBill = qFirstName.constrain("Bill").and(
				qLastName.constrain("TwoOK"));
		cAaron.or(cBill);
		ObjectSet results = qAuthors.execute();
		Assert.areEqual(4, results.size());
		while (results.hasNext()) {
			Person person = (Person) results.next();
			Assert.isTrue(person.lastName.endsWith("OK"));
		}
	}

}
