/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.classes.typedhierarchy;

public class STETH3 extends STETH2{
	
	String foo3;
	
	public STETH3(){
	}
	
	public STETH3(String str1, String str2, String str3){
		super(str1, str2);
		foo3 = str3;
	}
	
}

