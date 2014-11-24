/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.classes.untypedhierarchy;

public class STRUH2{
	
	Object parent;
	Object h3;
	String foo2;
	
	public STRUH2(){
	}
	
	public STRUH2(STRUH3 a3){
		h3 = a3;
		a3.parent = this;
	}
	
	public STRUH2(String str){
		foo2 = str;
	}
	
	public STRUH2(STRUH3 a3, String str){
		h3 = a3;
		a3.parent = this;
		foo2 = str;
	}
	
}

