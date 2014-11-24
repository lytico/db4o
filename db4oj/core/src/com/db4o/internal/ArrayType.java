/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

/**
 * @exclude
 */
public class ArrayType {
	
	public static final ArrayType NONE = new ArrayType(0);
	
	public static final ArrayType PLAIN_ARRAY = new ArrayType(3);
	
	public static final ArrayType MULTIDIMENSIONAL_ARRAY = new ArrayType(4);
	
	private ArrayType(int value){
		_value = value;
	}
	
	private final int _value;
	
	public int value(){
		return _value;
	}
	
	public static ArrayType forValue(int value){
		switch(value){
			case 0:
				return NONE;
			case 3:
				return PLAIN_ARRAY;
			case 4:
				return MULTIDIMENSIONAL_ARRAY;
			default:
				throw new IllegalArgumentException();
		}
		
	}
	
}
