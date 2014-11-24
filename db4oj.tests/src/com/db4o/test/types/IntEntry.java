/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class IntEntry extends TEntry {
	
	public TEntry firstElement(){
		return new TEntry(new Integer(101), "firstvalue");
	}

	public TEntry lastElement(){
		return new TEntry(new Integer(9999999), new ObjectSimplePublic("lastValue"));
	}

	public TEntry noElement(){
		return new TEntry(new Integer(-99999), "babe");
	}

	public TEntry[] test(int ver){
		if(ver == 1){
			return new TEntry[]{
				firstElement(),
				new TEntry(new Integer(111), new ObjectSimplePublic("111")),
				new TEntry(new Integer(9999111), new Double(0.4566)),
				lastElement()
			};
		}
		return new TEntry[]{
			new TEntry(new Integer(222), new ObjectSimplePublic("111")),
			new TEntry(new Integer(333), "TrippleThree"),
			new TEntry(new Integer(4444), new ObjectSimplePublic("4444")),
		};
	}
}