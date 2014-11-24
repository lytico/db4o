/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.config;

import com.db4o.*;
import com.db4o.reflect.jdk.*;

/**
 * @exclude
 * 
 * @sharpen.ignore
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
		return JdkReflector.toNative(oc.ext().reflector().forName((String)storedObject));
	}

	public Class storedClass(){
		return String.class;
	}
}
