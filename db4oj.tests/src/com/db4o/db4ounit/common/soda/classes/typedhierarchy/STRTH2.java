/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.classes.typedhierarchy;

public class STRTH2{
	
	public STRTH1TestCase parent;
	public STRTH3 h3;
	public String foo2;
	
	public STRTH2(){
	}
	
	public STRTH2(STRTH3 a3){
		h3 = a3;
		a3.parent = this;
	}
	
	public STRTH2(String str){
		foo2 = str;
	}
	
	public STRTH2(STRTH3 a3, String str){
		h3 = a3;
		a3.parent = this;
		foo2 = str;
	}
}

