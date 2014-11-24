/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.reflect.jdk;

import java.lang.reflect.*;

import com.db4o.reflect.*;
import com.db4o.reflect.core.*;

/**
 * Reflection implementation for Array to map to JDK reflection.
 * 
 * @sharpen.ignore
 */
public class JdkArray extends AbstractReflectArray {
    
    JdkArray(Reflector reflector){
        super(reflector);
    }
    
    public void analyze(Object obj, ArrayInfo info) {
        // do nothing
        // possible further processing here:  
        // Analyze component type, length, dimensions, primitive ... 
    }
    
    public Object newInstance(ReflectClass componentType, int length) {
        return Array.newInstance(JdkReflector.toNative(componentType), length);
    }

    public Object newInstance(ReflectClass componentType, int[] dimensions) {
        Class native1 = JdkReflector.toNative(componentType);
        return Array.newInstance(native1, dimensions);
    }

    public Object newInstance(ReflectClass componentType, ArrayInfo info) {
        Class clazz = JdkReflector.toNative(componentType);
        if(info instanceof MultidimensionalArrayInfo){
            return Array.newInstance(clazz, ((MultidimensionalArrayInfo)info).dimensions());
        }
        return Array.newInstance(clazz, info.elementCount());
    }


}
