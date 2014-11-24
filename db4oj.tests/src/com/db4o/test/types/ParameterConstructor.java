/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import com.db4o.*;
import com.db4o.test.*;

public class ParameterConstructor implements RTestable
{
	public String s;

    public ParameterConstructor(int a) {
    }

	public void compare(ObjectContainer con, Object obj, int ver){
		Compare.compare(con, set(newInstance(), ver), obj,"",null);
	}
	public boolean equals(Object obj){
		if(obj != null){
			if(obj instanceof ParameterConstructor){
				return s.equals(((ParameterConstructor)obj).s);
			}
		}
		return false;
	}
	
	
	public Object newInstance(){
		return new ParameterConstructor(5);	}

	
	public Object set(Object obj, int ver){		((ParameterConstructor)obj).set(ver);		return obj;
	}
	
	public void set(int ver){
		s = "set" + ver;
	}
	
	public boolean jdk2(){
		return false;
	}
	
	public boolean ver3(){
		return false;	}
}
