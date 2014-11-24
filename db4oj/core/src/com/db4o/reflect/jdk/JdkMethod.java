/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.reflect.jdk;

import java.lang.reflect.*;

import com.db4o.internal.*;
import com.db4o.reflect.*;

/**
 * @exclude
 * 
 * @sharpen.ignore
 */
public class JdkMethod implements ReflectMethod{
	
	private final Method method;
    private Reflector reflector;
	
	public JdkMethod(Method method_, Reflector reflector_){
		method = method_;
        reflector = reflector_;
	}
	
	public Object invoke(Object onObject, Object[] params) throws ReflectException {
		return Reflection4.invoke(params, onObject, method);
	} 

    public ReflectClass getReturnType() {
        return reflector.forClass(method.getReturnType());
    }
    
}
