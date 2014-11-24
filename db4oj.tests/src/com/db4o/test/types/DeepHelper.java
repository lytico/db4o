/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class DeepHelper
{
	public ObjectSimplePublic d1;

	public DeepHelper(){
		d1 = new ObjectSimplePublic();
	}

	public void set(int ver){

		if(ver == 1){
			d1.name = "OneONEOneONEOneONEOneONEOneONEOneONE";
		}else{
			d1.name = "TwoTWOTwoTWOTwoTWOTwoTWOTwoTWO";
		}
	}
}
