/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.j2me.bloat.testdata;

public class Dog extends Animal {
	private Dog[] _parents;
	private int _age;
	private int[] _prices;
	
	public Dog(String name,int age,Dog[] parents,int[] prices) {
		super(name);
		_age=age;
		_parents=parents;
		_prices=prices;
	}

	public int age() {
		return _age;
	}

	public Dog[] parents() {
		return _parents;
	}
	
	public int[] prices() {
		return _prices;
	}
	
	public String toString() {
		return "DOG: "+name()+"/"+age()+"/"+(_parents!=null ? String.valueOf(_parents.length) : "null")+" parents/"+(_prices!=null ? String.valueOf(_prices.length) : "null")+" prices";
	}
}
