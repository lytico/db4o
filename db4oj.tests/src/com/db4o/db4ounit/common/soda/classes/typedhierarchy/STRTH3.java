/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.classes.typedhierarchy;

public class STRTH3{
	
	public STRTH1TestCase grandParent;
	public STRTH2 parent;
	
	public String foo3;
	
	public STRTH3(){
	}
	
	public STRTH3(String str){
		foo3 = str;
	}
}

