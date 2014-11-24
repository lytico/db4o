/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import java.util.*;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.test.*;


public class RHashtable implements RTestable{

	public Object newInstance(){
		return new Hashtable();
	}

	public Object set(Object obj, int ver){
		TEntry[] arr = new TEntry().test(ver);
		Hashtable ht = (Hashtable)obj;
		ht.clear();
		for(int i = 0; i < arr.length; i ++){
			ht.put(arr[i].key, arr[i].value);
		}
		return obj;
	}

	public void compare(ObjectContainer con, Object obj, int ver){
		Hashtable ht = (Hashtable)obj;
		TEntry[] entries = new TEntry[ht.size()];
		Enumeration enu = ht.keys();
		int i = 0;
		while(enu.hasMoreElements()){
			entries[i] = new TEntry();
			entries[i].key = enu.nextElement();
			i++;
		}
		for(i = 0; i < entries.length; i ++){
			entries[i].value = ht.get(entries[i].key);
		}
		new TEntry().compare(entries, ver, false);
	}

	public void specific(ObjectContainer con, int step){
		TEntry entry = new TEntry().firstElement();
		Hashtable ht = (Hashtable)newInstance();
		if(step > 0){
			ht.put(entry.key, entry.value);
			ObjectSet set = con.queryByExample(ht);
			Collection4 col = new Collection4();
			while(set.hasNext()){
				Object obj = set.next();
				if(obj.getClass() == ht.getClass()){
					col.add(obj);
				}
			}
			if(col.size() != step){
				Regression.addError("Hashtable member query not found");
			}
		}
		entry = new TEntry().noElement();
		ht.put(entry.key, entry.value);
		if(con.queryByExample(ht).size() != 0){
			Regression.addError("Hashtable member query found too many");
		}
	}


	public boolean jdk2(){
		return false;
	}
	
	public boolean ver3(){
		return false;	}

}
