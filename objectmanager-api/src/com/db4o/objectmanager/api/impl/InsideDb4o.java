/* Copyright (C) 2006  db4objects Inc.  http://www.db4o.com */

package com.db4o.objectmanager.api.impl;

import com.db4o.*;
import com.db4o.internal.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.classindex.*;
import com.db4o.reflect.*;

/**
 * This class uses inside-methods of db4o to provide ObjectManager
 * functionality. All functionality used here should go into the
 * public db4o API.
 */
public class InsideDb4o {

	private final ObjectContainerBase _stream;

	public InsideDb4o(ObjectContainer oc){
		_stream = (ObjectContainerBase)oc;
	}

	/**
	 * This should go into StoredClass.
	 */
	public int getNumberOfObjectsForClass(String name) {

		if(_stream.isClient()){
			// no efficient way to do this for now.
			// A dedicated method would have to be sent.
			// todo: run it the old way with .size() in a separate thread and update the size info when done
			return 0;
		}

		try{
			return index(classMetadataForName(name)).size(trans());
		}catch (Exception e){
			e.printStackTrace();
		}
		return 0;
	}

    private ObjectContainerBase stream(){
    	return _stream;
    }

    private ClassMetadata classMetadataForName(String name){
    	return stream().classMetadataForReflectClass(reflectClass(name));
    }

    private ReflectClass reflectClass(String name){
    	return stream().reflector().forName(name);
    }

    private BTree index(ClassMetadata yapClass) throws ClassCastException, NullPointerException{
    	if (yapClass.index() != null) {
			return ((BTreeClassIndexStrategy) yapClass.index()).btree();
		} else {
			System.err.println("CLASS INDEX IS NULL FOR: " + yapClass.getName());
		}
		return null;
    }
    
    private Transaction trans(){
    	return stream().transaction();
    }

}
