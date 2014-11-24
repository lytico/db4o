/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.classes.typedhierarchy;

public class STETH2 extends STETH1{
	
	String foo2;
	
	public STETH2(){
	}
	
	public STETH2(String str1, String str2){
		super(str1);
		foo2 = str2;
	}
}

