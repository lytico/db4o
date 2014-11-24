/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import com.db4o.*;
import com.db4o.config.*;

public class TCustomConstructor implements ObjectConstructor
{
	public Object onStore(ObjectContainer con, Object obj){
		CustomConstructor cc = (CustomConstructor)obj;
		String[] strings = new String[2];
		strings[0] = cc.name;
		strings[1] = cc.tname;
		return strings;
	}

	public void onActivate(ObjectContainer con, Object obj, Object members){
		// do nothing. All is done in onInstantiate
	}

	public Object onInstantiate(ObjectContainer container, Object storedObject){
		String[] strings = (String[])storedObject;
		CustomConstructor cc = new CustomConstructor(strings[1]);
		cc.name = strings[0];
		return cc;
	}

	public Class storedClass(){
		return String[].class;
	}
}
