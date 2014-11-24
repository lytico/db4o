/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.config;

import java.util.*;

import com.db4o.*;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TCollection implements ObjectTranslator {
	
	public Object onStore(ObjectContainer con, Object object){
		Collection col = (Collection)object;
		Object[] elements = new Object[col.size()];
		Iterator it = col.iterator();
		int i = 0;
		while(it.hasNext()){
			elements[i++] = it.next();
		}
		return elements;
	}

	public void onActivate(ObjectContainer con, Object object, Object members){
		Collection col = (Collection)object;
		col.clear();
		if(members != null){
			Object[] elements = (Object[]) members;
			for(int i = 0; i < elements.length; i++){
				col.add(elements[i]);
			}
		}
	}

	public Class storedClass(){
		return Object[].class;
	}
}
