/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.config;

import java.util.*;

import com.db4o.*;

/**
 * @exclude
 * @sharpen.ignore
 */
public class TVector implements ObjectTranslator {
	
	public Object onStore(ObjectContainer con, Object object){
		Vector vt = (Vector)object;
		Object[] elements = new Object[vt.size()];
		Enumeration enumeration = vt.elements();
		int i = 0;
		while(enumeration.hasMoreElements()){
			elements[i++] = enumeration.nextElement();
		}
		return elements;
	}

	public void onActivate(ObjectContainer con, Object object, Object members){
		Vector vt = (Vector)object;
		vt.removeAllElements();
		if(members != null){
			Object[] elements = (Object[]) members;
			for(int i = 0; i < elements.length; i++){
				vt.addElement(elements[i]);
			}
		}
	}

	public Class storedClass(){
		return Object[].class;
	}
}
