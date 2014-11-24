/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import java.net.*;

import com.db4o.config.*;
import com.db4o.config.annotations.reflect.*;
import com.db4o.ext.*;
import com.db4o.reflect.*;
import com.db4o.reflect.jdk.*;

/**
 * @sharpen.ignore
 */
@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
class JDK_5 extends JDK_1_4 {

	@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
	public final static class Factory implements JDKFactory {
		public JDK tryToCreate() {
	    	if(!classIsAvailable("java.lang.Enum")){
	    		return null;
	    	}
	    	return new JDK_5();
		}
	}
	
	public Config4Class extendConfiguration(ReflectClass clazz,
			Configuration config, Config4Class classConfig) {
		Class javaClazz = JdkReflector.toNative(clazz);
		if(javaClazz==null) {
			return classConfig;
		}
		try {
			ConfigurationIntrospector instrospetor = new ConfigurationIntrospector(javaClazz, config, classConfig);
			return instrospetor.apply();
		} catch (Exception exc) {
			throw new Db4oException(exc);
		}
	}
    
    public boolean isConnected(Socket socket){
        if(socket == null){
            return false;
        }
        if(! socket.isConnected() ){
            return false;
        }
        return ! socket.isClosed();
    }

	boolean isEnum(Reflector reflector, ReflectClass claxx) {

		if (claxx == null) {
			return false;
		}

		final ReflectClass enumClass = reflector.forClass(java.lang.Enum.class);
		return enumClass.isAssignableFrom(claxx);
	}
	
	public long nanoTime() {
		return System.nanoTime();
	}
	
	public boolean useNativeSerialization() {
		return false;
	}

	public int ver() {
	    return 5;
	}
	
	public void throwIllegalArgumentException(Throwable origExc) {
		throw new IllegalArgumentException("Argument not an unchecked Exception.", origExc);
	}

}
