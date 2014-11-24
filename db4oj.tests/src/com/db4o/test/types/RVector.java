/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import java.util.*;

import com.db4o.*;
import com.db4o.test.*;

public class RVector implements RTestable{

	public Object newInstance(){
		return new Vector();
	}

	public Object set(Object obj, int ver){
		TEntry[] arr = new TEntry().test(ver);
		Vector vt = (Vector)obj;
		vt.removeAllElements();
		for(int i = 0; i < arr.length; i ++){
			vt.addElement(arr[i].key);
		}
		return obj;
	}

	public void compare(ObjectContainer con, Object obj, int ver){
		Vector vt = (Vector)obj;
		TEntry[] entries = new TEntry[vt.size()];
		Enumeration enu = vt.elements();
		int i = 0;
		while(enu.hasMoreElements()){
			entries[i] = new TEntry();
			entries[i].key = enu.nextElement();
			i++;
		}
		new TEntry().compare(entries, ver, true);
	}

	public void specific(ObjectContainer con, int step){
		TEntry entry = new TEntry().firstElement();
		Vector vt = (Vector)newInstance();
		if(step > 0){
			vt.addElement(entry.key);
			ObjectSet set = con.queryByExample(vt);
			if(set.size() != step){
				Regression.addError("Vector member query not found");
			}else{
				Vector res = (Vector)set.next();
				if(! (res.firstElement().equals(new TEntry().firstElement().key))){
					Regression.addError("Vector order changed.");
				}
			}
		}
		entry = new TEntry().noElement();
		vt.addElement(entry.key);
		if(con.queryByExample(vt).size() != 0){
			Regression.addError("Vector member query found too many");
		}
	}


	public boolean jdk2(){
		return false;
	}
	
	public boolean ver3(){
		return false;	}

}
