package com.db4o.browser.test;

public class TestData {
	int id;
	String name;
	TestData previous;
	
	public TestData(int id, String name, TestData previous) {
		super();
		this.id = id;
		this.name = name;
		this.previous = previous;
	}
}
