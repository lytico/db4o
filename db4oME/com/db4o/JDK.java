/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import java.util.*;

import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;
import com.db4o.reflect.self.*;
import com.db4o.types.*;

/**
 * @exclude
 */
public class JDK {
	Thread addShutdownHook(Runnable a_runnable){
		return null;
	}
	
	Db4oCollections collections(YapStream a_stream){
	    return null;
	}
    
    Class constructorClass(){
        return null;
    }
	
	Object createReferenceQueue() {
		return null;
	}

    public Object createWeakReference(Object obj){
        return obj;
    }
    
	Object createYapRef(Object a_queue, YapObject a_yapObject, Object a_object) {
		return null;
	}
	
    Object deserialize(byte[] bytes) {
    	throw new Db4oException(Messages.NOT_IMPLEMENTED);
    }

	void forEachCollectionElement(Object a_object, Visitor4 a_visitor) {
        if(! Deploy.csharp){
            Enumeration e = null;
            if (a_object instanceof Hashtable) {
                e = ((Hashtable)a_object).elements();
            } else if (a_object instanceof Vector) {
                e = ((Vector)a_object).elements();
            }
            if (e != null) {
                while (e.hasMoreElements()) {
                    a_visitor.visit(e.nextElement());
                }
            }
        }
	}
	
	String format(Date date, boolean showTime) {
		return date.toString();
	}
	
	Object getContextClassLoader(){
		return null;
	}

	Object getYapRefObject(Object a_object) {
		return null;
	}
    
    boolean isCollectionTranslator(Config4Class a_config) {
        if(!Deploy.csharp){
            if (a_config != null) {
                ObjectTranslator ot = a_config.getTranslator();
                if (ot != null) {
                    return ot instanceof THashtable;
                }
            }
        }
        return false;
    }

	public int ver(){
	    return 1;
	}
	
	void killYapRef(Object obj){
		
	}
	
	synchronized void lockFile(Object file) {
	}
	
    /**
     * use for system classes only, since not ClassLoader
     * or Reflector-aware
     */
	boolean methodIsAvailable(
            String className,
            String methodName,
            Class[] params) {
    	return false;
    }

	void pollReferenceQueue(YapStream a_stream, Object a_referenceQueue) {
	}
	
	public void registerCollections(GenericReflector reflector) {
		
	}
	
	void removeShutdownHook(Thread a_thread){
		
	}
	
	public Object serializableConstructor(Class clazz){
	    return null;
	}
	
    byte[] serialize(Object obj) throws Exception{
    	throw new Db4oException(Messages.NOT_IMPLEMENTED);
    }

	void setAccessible(Object a_accessible) {
	}
    
    boolean isEnum(Reflector reflector, ReflectClass clazz) {
        return false;
    }
	
	synchronized void unlockFile(Object file) {
	}
    
    public Object weakReferenceTarget(Object weakRef){
        return weakRef;
    }
    
    public Reflector createReflector(Object classLoader) {
    	return new NullReflector();
    }
}
