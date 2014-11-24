/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import java.lang.ref.*;

import com.db4o.config.*;
import com.db4o.foundation.*;
import com.db4o.internal.handlers.*;
import com.db4o.internal.handlers.net.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;

/**
 * @sharpen.ignore
 */
@decaf.Remove(decaf.Platform.JDK11)
class JDK_1_2 extends JDKReflect {
	
	JDK_1_2(){
	}
	
	@decaf.Remove(decaf.Platform.JDK11)
	public final static class Factory implements JDKFactory {
		public JDK tryToCreate() {
	    	if(!classIsAvailable(Platform4.ACCESSIBLEOBJECT)){
	    		return null;
	    	}
	    	return new JDK_1_2();
		}
	}
	
	public Class loadClass(String className, Object loader) throws ClassNotFoundException {
		if(loader == null) {
			loader = getClass().getClassLoader();
		}
		return Class.forName(className, false, (ClassLoader)loader);
	}
	
    Object createReferenceQueue() {
        return new ReferenceQueue4();
    }

    public Object createWeakReference(Object obj){
        return new WeakReference(obj);
    }
    
    Object createActivateObjectReference(Object referenceQueue, ObjectReference objectReference, Object obj) {
        return new ActiveObjectReference(referenceQueue, objectReference, obj);
    }
    
    public void extendConfiguration(Config4Impl config) {
        new TypeHandlerConfigurationJDK_1_2(config).apply();
    }

    void forEachCollectionElement(Object a_object, Visitor4 a_visitor) {
        java.util.Iterator i = null;
        if (a_object instanceof java.util.Collection) {
            i = ((java.util.Collection) a_object).iterator();
        } else if (a_object instanceof java.util.Map) {
            i = ((java.util.Map) a_object).keySet().iterator();
        }
        if (i != null) {
            while (i.hasNext()) {
                a_visitor.visit(i.next());
            }
        }
    }

    Object getContextClassLoader() {
        return Thread.currentThread().getContextClassLoader();
    }

    Object getYapRefObject(Object a_object) {
        if (a_object instanceof ActiveObjectReference) {
            return ((ActiveObjectReference) a_object).get();
        }
        return a_object;
    }
    
    boolean isCollectionTranslator(Config4Class config) {
        if (config != null) {
            ObjectTranslator ot = config.getTranslator();
            if (ot != null) {
                return ot instanceof TCollection || ot instanceof TMap || ot instanceof THashtable;
            }
        }
        return false;
    }
    
    public int ver(){
        return 2;
    }
    
	void killYapRef(Object obj){
		if(obj instanceof ActiveObjectReference){
			((ActiveObjectReference)obj)._referent = null;
		}
	}

    void pollReferenceQueue(ObjectContainerBase container, Object referenceQueue) {
        if (referenceQueue == null) {
        	return;
        }
        
        ReferenceQueue4 queue = (ReferenceQueue4) referenceQueue;
        ActiveObjectReference ref;
        synchronized(container.lock()){
            while ((ref = queue.pollObjectReference()) != null) {
                container.removeFromAllReferenceSystems(ref._referent);
            }
        }
    }
    
	public void registerCollections(GenericReflector reflector) {
		reflector.registerCollection(java.util.Collection.class);
		reflector.registerCollection(java.util.Map.class);
	}

    void setAccessible(Object accessible) {
		try {
			((java.lang.reflect.AccessibleObject) accessible)
					.setAccessible(true);
		} catch (SecurityException e) {

		}
	}
    
    public NetTypeHandler[] netTypes(Reflector reflector) {
        return new NetTypeHandler[] {
            new NetDateTime(reflector),
            new NetDecimal(reflector),
            new NetSByte(reflector),
            new NetUInt(reflector),
            new NetULong(reflector),
            new NetUShort(reflector)
          };
    }
    
    public Object weakReferenceTarget(Object weakRef){
        if(weakRef instanceof WeakReference){
            return ((WeakReference)weakRef).get();
        }
        return weakRef;
    }
    

}
