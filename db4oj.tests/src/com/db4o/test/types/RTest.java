/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import com.db4o.*;
import com.db4o.test.*;

public abstract class RTest implements RTestable{	
	public Object newInstance(){		try{
			return this.getClass().newInstance();
		}catch(Exception e){			return null;
		}	}	
	public Object set(Object obj, int ver){
		((RTest)obj).set(ver);
		return obj;
	}	
	public abstract void set(int ver);
	
	public void compare(ObjectContainer con, Object obj, int ver){
		Compare.compare(con, set(newInstance(), ver), obj,"",null);
	}
	
	public boolean jdk2(){
		return false;
	}
		public boolean ver3(){
		return false;	}
}