/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public final class YapBit{
	
	private int i_value;
	
	public YapBit(int a_value){
		i_value = a_value;
	}
	
	void set(boolean a_bit){
		i_value = i_value * 2;
		if(a_bit){
			i_value ++;
		}
	}
	
	public boolean get(){
		double cmp = (double)i_value / 2;
		i_value = i_value / 2;
		return (cmp != i_value);
	}
	
	byte getByte(){
		return (byte)i_value;
	}
}
