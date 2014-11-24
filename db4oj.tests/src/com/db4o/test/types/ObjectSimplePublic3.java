/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class ObjectSimplePublic3 extends RTest
{
	public String name;


	public ObjectSimplePublic3(){
	}

	public void set(int ver){
		if(ver == 1){
			name = "OneONEOneONEOneONEOneONEOneONEOneONE";	
		}else{
			name = "TwoTWOTwoTWOTwoTWOTwoTWOTwoTWOto";	
		}
	}
}
