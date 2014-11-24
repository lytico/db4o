/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class InterfacePrivate extends RTest
{
	private InterfaceHelper oo;

	public void set(int ver){
		oo = new ObjectSimplePrivate();	
		((ObjectSimplePrivate)oo).set(ver);
	}

	public boolean jdk2(){
		return true;
	}

}
