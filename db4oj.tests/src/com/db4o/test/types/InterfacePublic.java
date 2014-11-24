/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import com.db4o.*;
import com.db4o.test.*;

public class InterfacePublic implements RTestable
{
	public InterfaceHelper oo;
	
	public void compare(ObjectContainer con, Object obj, int ver){
		Compare.compare(con, set(newInstance(), ver), obj,"",null);
	}		public Object newInstance(){
		return new InterfacePublic();	}
	
	public Object set(Object obj, int ver){		((InterfacePublic)obj).set(ver);		return obj;
	}
	public void set(int ver){
		oo = new ObjectSimplePublic();	
		((ObjectSimplePublic)oo).set(ver);
	}
	
	public boolean jdk2(){
		return false;
	}
	
	public boolean ver3(){
		return false;	}

}
