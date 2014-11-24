/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.classes.typedhierarchy;

public class STETH4 extends STETH2{
	
	String foo4;
	
	public STETH4(){
	}
	
	public STETH4(String str1, String str2, String str3){
		super(str1, str2);
		foo4 = str3;
	}
	
}

