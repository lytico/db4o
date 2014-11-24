/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.soda.arrays;

import com.db4o.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

//COR-1977
public class ArrayDescendSubQueryTestCase extends AbstractDb4oTestCase {
	
	public static class Person {
		public String _name;

		public Person(String name) {
			_name = name;
		}
		
		@Override
		public String toString() {
			return _name;
		}
	}

	public static class Book {
		public String _title;
		public Person _author;
		public Book[] _cites;

		public Book(String title, Person author, Book[] cites) {
			_title = title;
			_author = author;
			_cites = cites;
		}
		
		@Override
		public String toString() {
			return _title;
		}
	}
	
	@Override
	protected void store() throws Exception {
		Person erich = new Person("Erich");
		Person kent = new Person("Kent");
		Person bill = new Person("Bill");		
		Book gof = new Book("gof", erich, new Book[0]);
		Book xp = new Book("xp", kent, new Book[] { gof });
		Book ddd = new Book("ddd", bill, new Book[] { gof, xp });
		store(ddd);
	}
	
	// all books cited in ddd - works
	public void testSimpleDescend() {
		Query topQuery = newQuery(Book.class);
		topQuery.descend("_title").constrain("ddd");
		Query subQuery = topQuery.descend("_cites");
		Assert.areEqual(2, subQuery.execute().size());
	}

	// all authors of books cited in ddd - only selects one array element as root for the second descend
	public void testDoubleDescend() {
		Query topQuery = newQuery(Book.class);
		topQuery.descend("_title").constrain("ddd");
		Query subQuery = topQuery.descend("_cites").descend("_author");
		ObjectSet<Object> result = subQuery.execute();
//		while(result.hasNext()) {
//			System.out.println(result.next());
//		}
		Assert.areEqual(2, result.size());
	}
}
