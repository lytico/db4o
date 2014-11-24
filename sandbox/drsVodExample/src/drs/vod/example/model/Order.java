/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package drs.vod.example.model;


public class Order {
	
	private Book book;
	
	private Customer customer;
	
	public Order(){
		
	}

	public Order(Book book, Customer customer) {
		this.book = book;
		this.customer = customer;
	}

	@Override
	public String toString() {
		return "Order [book=" + book + ", customer=" + customer + "]";
	}

}
