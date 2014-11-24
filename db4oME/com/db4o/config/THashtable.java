/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.config;

import java.util.*;

import com.db4o.*;

/**
 * @exclude
 */
public class THashtable implements ObjectTranslator
{
	public Object onStore(ObjectContainer con, Object object){
		Hashtable ht = (Hashtable)object;
		Entry[] entries = new Entry[ht.size()];
		Enumeration enumeration = ht.keys();
		int i = 0;
		while(enumeration.hasMoreElements()){
			entries[i] = new Entry();
			entries[i].key = enumeration.nextElement();
			entries[i].value = ht.get(entries[i].key);
			i++;
		}
		return entries;
	}

	public void onActivate(ObjectContainer con, Object object, Object members){
		Hashtable ht = (Hashtable)object;
		ht.clear();
		if(members != null){
			Entry[] entries = (Entry[]) members;
			for(int i = 0; i < entries.length; i++){
                if(entries[i] != null){
    				if(entries[i].key != null && entries[i].value != null){
    					ht.put(entries[i].key,entries[i].value);
    				}
                }
			}
		}
	}

	public Class storedClass(){
		return Entry[].class;
	}
}
