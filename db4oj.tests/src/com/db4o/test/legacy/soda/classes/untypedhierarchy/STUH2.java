/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.classes.untypedhierarchy;

public class STUH2{
	
	Object h3;
	Object foo2;
	
	public STUH2(){
	}
	
	public STUH2(STUH3 a3){
		h3 = a3;
	}
	
	public STUH2(String str){
		foo2 = str;
	}
	
	public STUH2(STUH3 a3, String str){
		h3 = a3;
		foo2 = str;
	}
}

