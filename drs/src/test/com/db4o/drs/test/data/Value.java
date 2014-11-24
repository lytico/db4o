package com.db4o.drs.test.data;

/**
 * @sharpen.struct
 */
public class Value
{
	public int value;
	
	public Value(int value) {
		this.value = value;
	}
	
	public boolean equals(Object obj) {
		if (!(obj instanceof Value)) {
			return false;
		}
		Value other = (Value)obj;
		return other.value == value;
	}
}