/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.reflect.core;

import com.db4o.foundation.*;
import com.db4o.reflect.*;


/**
 * @exclude
 */
public class ReflectorUtils {
	
	public static ReflectClass reflectClassFor(Reflector reflector, Object clazz) {
        
       if(clazz instanceof ReflectClass){
            return (ReflectClass)clazz;
        }
        
        if(clazz instanceof Class){
            return reflector.forClass((Class)clazz);
        }
        
        if(clazz instanceof String){
            return reflector.forName((String)clazz);
        }
        
        return reflector.forObject(clazz);
    }
	
	public static ReflectField field(ReflectClass claxx, String name){
		while(claxx!=null) {
			try {
				return claxx.getDeclaredField(name);
			} catch (Exception e) {
				
			}
			claxx=claxx.getSuperclass();
		}
		return null;
	}
	
	public static void forEachField(ReflectClass claxx, Procedure4<ReflectField> procedure){
		while(claxx!=null) {
			final ReflectField[] declaredFields = claxx.getDeclaredFields();
			for (ReflectField reflectField : declaredFields) {
				procedure.apply(reflectField);
			}
			claxx=claxx.getSuperclass();
		}
	}

}
