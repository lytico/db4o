/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import com.db4o.*;
import com.db4o.test.*;

public class SelfReference implements InterfaceHelper, RTestable
{
	public SelfReference self;
	public String name;

	public SelfReference(){
		self = this;
	}

	public SelfReference(String a_name){
		this();
		name = a_name;
	}
		
	public void compare(ObjectContainer con, Object obj, int ver){
		Compare.compare(con, set(newInstance(), ver), obj,"", null);
	}
	public boolean equals(Object obj){
		if(obj != null){
			if(obj instanceof SelfReference){
				if(name != null){
					return name.equals(((SelfReference)obj).name);
				}
			}
		}
		return false;
	}
	
	public Object newInstance(){
		return new SelfReference();	}

	
	public Object set(Object obj, int ver){
		((SelfReference)obj).set(ver);		return obj;
	}

	public void set(int ver){
		if(ver == 1){
			name = "OneONEOneONEOneONEOneONEOneONEOneONE";	
		}else{
			name = "TwoTWOTwoTWOTwoTWOTwoTWOTwoTWO";	
		}
	}

	public boolean jdk2(){
		return false;
	}
	
	public boolean ver3(){
		return false;	}
}
