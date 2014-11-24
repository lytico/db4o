/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test;

import com.db4o.*;
import com.db4o.test.types.*;

public class ExtendsDate extends java.util.Date implements RTestable
{
	public ExtendsDate(){
		super();
	}

	public void compare(ObjectContainer con, Object obj, int ver){
		Compare.compare(con, set(newInstance(), ver), obj,"", null);
	}

	public boolean equals(Object obj){
		if(obj != null){
			if(obj instanceof ExtendsDate){
				return getTime() == ((ExtendsDate)obj).getTime();
			}
		}
		return false;
	}

	public Object newInstance(){
		return new ExtendsDate();
	}


	public Object set(Object obj, int ver){
		((ExtendsDate)obj).set(ver);
		return obj;
	}

	public void set(int ver){
		setTime(ver);
	}

	public boolean jdk2(){
		return false;
	}

	public boolean ver3(){
		return false;
	}

}
