/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package drs.vod.example.model;

public class Book {
	
	private String title;
	
	private float price;

	public Book(){
		
	}
	
	public Book(String title, float price) {
		this.title = title;
		this.price = price;
	}
	
	@Override
	public String toString() {
		return "Book [title=" + title + ", price=" + price + "]";
	}
	
	public float price(){
		return price;
	}
	
	public void price(float price){
		this.price = price;
	}
	

}
