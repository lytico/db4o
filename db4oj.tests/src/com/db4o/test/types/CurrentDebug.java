/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class CurrentDebug extends RTest
{
	private Byte nByte;
	
	public void set(int ver){
		if(ver == 1){
			nByte = new Byte((byte)3);
		}else{
			nByte = new Byte((byte)(Byte.MIN_VALUE + 1));
		}
	}
	
	public boolean jdk2(){
		return true;
	}
}
