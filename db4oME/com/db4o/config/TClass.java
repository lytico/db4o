/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.config;

import com.db4o.*;

/**
 * @exclude
 */
public class TClass implements ObjectConstructor
{
	public Object onStore(ObjectContainer oc, Object object){
		return ((Class)object).getName();
	}
	
	public void onActivate(ObjectContainer oc, Object object, Object members){
		// do nothing
	}

	public Object onInstantiate(ObjectContainer oc, Object storedObject){
		try{
			return Class.forName((String)storedObject);
		}catch(Exception e){}
		return null;
	}

	public Class storedClass(){
		return String.class;
	}
}
