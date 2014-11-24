/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.test.types.*;

public class RenTwo implements InterfaceHelper, RTestable
{
	public String s1;
	public String s2;

	public void compare(ObjectContainer con, Object obj, int ver){
		Compare.compare(con, set(newInstance(), ver), obj,"", null);
	}
	public boolean equals(Object obj){
		return(obj != null &&
			   obj instanceof RenTwo &&
			   s1 != null && 
			   s2 != null &&
			   s1.equals(((RenTwo)obj).s1) &&
			   s2.equals(((RenTwo)obj).s2)
		);
	}
	
	public Object newInstance(){
		return new RenTwo();	}

	
	public Object set(Object obj, int ver){
		((RenTwo)obj).set(ver);		return obj;
	}

	public void set(int ver){
		if(ver == 1){
			s1 = "One";
			s2 = "One";
		}else{
			s1 = "Two";
			s2 = "Two";
		}
	}

	public boolean jdk2(){
		return false;
	}
	
	public boolean ver3(){
		return false;	}

}
