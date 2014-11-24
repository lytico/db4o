/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

public class CustomConstructor extends RTest
{
	public String name;
	transient String tname;

	public CustomConstructor(){
	}
	
	public CustomConstructor(String transientName){
		tname = transientName;
	}
	
	public boolean equals(Object obj){
		if(obj != null){
			if(obj instanceof CustomConstructor){
				CustomConstructor cc = (CustomConstructor)obj;
				if(name != null){
					if (! name.equals(cc.name)){
						return false;
					}
					if(cc.name != null){
						return false;
					}
				}
				if(tname != null){
					if(! tname.equals(cc.tname)){
						return false;
					}
					if(cc.tname != null){
						return false;
					}
				}
				return true;
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
		tname = name;
	}
}
