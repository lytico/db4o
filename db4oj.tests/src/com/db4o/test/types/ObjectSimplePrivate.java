/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class ObjectSimplePrivate extends RTest implements InterfaceHelper
{
	private String name;

	public ObjectSimplePrivate(){
	}

	public ObjectSimplePrivate(String a_name){
		name = a_name;
	}

	public boolean equals(Object obj){
		if(obj != null){
			if(obj instanceof ObjectSimplePrivate){
				if(name != null){
					return name.equals(((ObjectSimplePrivate)obj).name);
				}
			}
		}
		return false;
	}


	public void set(int ver){
		if(ver == 1){
			name = "OneONEOneONEOneONEOneONEOneONEOneONE";
		}else{
			name = "TwoTWOTwoTWOTwoTWOTwoTWOTwoTWO";
		}
	}

	public boolean jdk2(){
		return true;
	}
}
