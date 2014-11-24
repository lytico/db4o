/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package drs.vod.example;

import com.db4o.*;

import drs.vod.example.model.*;
import drs.vod.example.utils.*;

public class StoreNewOrderInDb4o {
	
	public static void main(String[] args) {
		
		ObjectContainer objectContainer = Db4oHelper.openObjectContainer();
		ObjectSet<Book> storedBooks = objectContainer.query(Book.class);
		
		if(storedBooks.size() == 0){
			System.out.println("There are no books in db4o yet.");
			System.out.println("Run the following Java classes first:");
			System.out.println(Store2NewBooksToVod.class.getSimpleName());
			System.out.println(ReplicateAllBooksOrdersCustomersFromVodToDb4o.class.getSimpleName());
			objectContainer.close();
			return;
		}
		Book book = storedBooks.get(0);
		
		Customer customer = null;
		ObjectSet<Customer> customers = objectContainer.query(Customer.class);
		if(customers.size() == 0){
			customer = new Customer("Alice");
			objectContainer.store(customer);
		} else {
			customer = customers.get(0);
		}
		
		Order order = new Order(book, customer);
		objectContainer.store(order);
		
		System.out.println("Order stored to db4o:");
		System.out.println(order);
		
		objectContainer.commit();
		objectContainer.close();
		
	}

}
