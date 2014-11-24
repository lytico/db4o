/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.config;

import java.util.*;

import com.db4o.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TTreeMap implements ObjectConstructor {
	
	public Object onStore(ObjectContainer con, Object object){
		return ((TreeMap)object).comparator();
	}

	public void onActivate(ObjectContainer con, Object object, Object members){
		// do nothing
	}

	public Object onInstantiate(ObjectContainer container, Object storedObject){
		if(storedObject instanceof Comparator) {
			return new TreeMap((Comparator)storedObject);
		}
		return new TreeMap();
	}

	public Class storedClass(){
		return Object.class;
	}
}
