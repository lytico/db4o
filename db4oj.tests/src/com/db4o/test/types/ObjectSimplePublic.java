/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import com.db4o.*;
import com.db4o.test.*;

public class ObjectSimplePublic implements InterfaceHelper, RTestable
{
	public String name;

	public ObjectSimplePublic(){
	}

	public ObjectSimplePublic(String a_name){
		name = a_name;
	}

	public void compare(ObjectContainer con, Object obj, int ver){
		Compare.compare(con, set(newInstance(), ver), obj,"", null);
	}

	public boolean equals(Object obj){
		if(obj != null){
			if(obj instanceof ObjectSimplePublic){
				if(name != null){
					return name.equals(((ObjectSimplePublic)obj).name);
				}
			}
		}
		return false;
	}

	public Object newInstance(){
		return new ObjectSimplePublic();
	}


	public Object set(Object obj, int ver){
		((ObjectSimplePublic)obj).set(ver);
		return obj;
	}


	public void set(int ver){
		/*
		km = new KillMe();
		km.set(ver);
		*/
		if(ver == 1){
			name = "OneONEOneONEOneONEOneONEOneONEOneONE";
		}else{
			name = "TwoTWOTwoTWOTwoTWOTwoTWOTwoTWOto";
		}
	}

	public boolean jdk2(){
		return false;
	}

	public boolean ver3(){
		return false;
	}

}
