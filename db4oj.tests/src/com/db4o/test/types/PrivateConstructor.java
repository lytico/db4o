/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import com.db4o.*;
import com.db4o.test.*;

public class PrivateConstructor implements RTestable
{
	public String s;

    private PrivateConstructor() {
    }

	public void compare(ObjectContainer con, Object obj, int ver){
		Compare.compare(con, set(newInstance(), ver), obj,"", null);
	}
	
	static public PrivateConstructor construct(){
		return new PrivateConstructor();	}

	public boolean equals(Object obj){
		if(obj != null){
			if(obj instanceof PrivateConstructor){
				return s.equals(((PrivateConstructor)obj).s);
			}
		} 
		return false;
	}
	
	public boolean jdk2(){
		return true;
	}
	
	public Object newInstance(){
		return new PrivateConstructor();	}
	
	public Object set(Object obj, int ver){		((PrivateConstructor)obj).set(ver);		return obj;
	}
	
	public void set(int ver){
		s = "set" + ver;
	}
	
	public boolean ver3(){
		return false;	}

}
