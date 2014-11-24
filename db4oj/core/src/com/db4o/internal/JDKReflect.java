/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import java.io.*;
import java.lang.reflect.*;
import java.math.*;
import java.text.*;
import java.util.*;

import com.db4o.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;
import com.db4o.reflect.jdk.*;


/**
 * package and class name are hard-referenced in JavaOnly#jdk()
 * @sharpen.ignore
 */
public class JDKReflect extends JDK {
	
	public final static class Factory implements JDKFactory {
		public JDK tryToCreate() {
	    	return new JDKReflect();
		}
	}

	
	/**
	 * always call super if you override
	 */
	public void commonConfigurations(Config4Impl config) {
		super.commonConfigurations(config);
		config.objectClass(BigDecimal.class).storeTransientFields(true);
		config.objectClass(BigInteger.class).storeTransientFields(true);
	}

    Class constructorClass(){
        return Constructor.class;
    }
    
    Object deserialize(byte[] bytes) {
        try {
            return new ObjectInputStream(new ByteArrayInputStream(bytes)).readObject();
        } catch (Exception e) {
        }
        return null;
    }
    
	String format(Date date, boolean showTime) {
        String fmt = "yyyy-MM-dd";
        if (showTime) {
            fmt += " HH:mm:ss";
        }
        return new SimpleDateFormat(fmt).format(date);
	}
	
	public Class loadClass(String className, Object loader) throws ClassNotFoundException {
        return (loader != null ? ((ClassLoader)loader).loadClass(className) : Class.forName(className));
    }

	
    /**
     * use for system classes only, since not ClassLoader
     * or Reflector-aware
     */
    final boolean methodIsAvailable(String className, String methodName, Class[] params) {
		return Reflection4.getMethod(className, methodName, params) != null;
	}
    
    @Override
    boolean supportSkipConstructorCall() {
		return methodIsAvailable(Platform4.REFLECTIONFACTORY, Platform4.GETCONSTRUCTOR, new Class[] { Class.class, constructorClass() });
    }
    
    public void registerCollections(GenericReflector reflector) {
        reflector.registerCollection(java.util.Vector.class);
        reflector.registerCollection(java.util.Hashtable.class);
    }
    
    byte[] serialize(Object obj) throws Exception{
        ByteArrayOutputStream byteStream = new ByteArrayOutputStream();
        new ObjectOutputStream(byteStream).writeObject(obj);
        return byteStream.toByteArray();
    }

    public Reflector createReflector(Object classLoader) {
    	if(classLoader==null) {
            
            // FIXME: The new reflector does not like the ContextCloader at all.
            //        Resolve hierarchies.
            // classLoader=getContextClassLoader();
            
            // if (cl == null || classloaderName.indexOf("eclipse") >= 0) {
                classLoader= Db4o.class.getClassLoader();
            // 
    	}
    	return new JdkReflector((ClassLoader)classLoader);
    }
    
    public Reflector reflectorForType(Class clazz) {
    	return createReflector(clazz.getClassLoader());
    }
}
