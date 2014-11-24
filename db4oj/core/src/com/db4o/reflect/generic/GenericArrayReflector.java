/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.reflect.generic;

import com.db4o.reflect.*;

/**
 * @exclude
 */
public class GenericArrayReflector implements ReflectArray{
    
    private final ReflectArray _delegate;
    
    public GenericArrayReflector(GenericReflector reflector){
        _delegate = reflector.getDelegate().array();
    }
    
    public void analyze(Object obj, ArrayInfo info) {
        _delegate.analyze(obj, info);
    }

    public int[] dimensions(Object arr) {
        return _delegate.dimensions(arr);
    }

    public int flatten(Object a_shaped, int[] a_dimensions, int a_currentDimension, Object[] a_flat, int a_flatElement) {
        return _delegate.flatten(a_shaped, a_dimensions, a_currentDimension, a_flat, a_flatElement);
    }

    public Object get(Object onArray, int index) {
        if(onArray instanceof GenericArray){
            return ((GenericArray)onArray)._data[index];
        }
        return _delegate.get(onArray, index);
    }

    public ReflectClass getComponentType(ReflectClass claxx) {
        claxx = claxx.getDelegate();
        if(claxx instanceof GenericClass){
            return claxx;
        }
        return _delegate.getComponentType(claxx);
    }

    public int getLength(Object array) {
        if(array instanceof GenericArray){
            return ((GenericArray)array).getLength();
        }
        return _delegate.getLength(array);
    }
    
    public boolean isNDimensional(ReflectClass a_class) {
        if(a_class instanceof GenericArrayClass){
            return false;
        }
        return _delegate.isNDimensional(a_class.getDelegate());
    }
    
    public Object newInstance(ReflectClass componentType, ArrayInfo info) {
        componentType = componentType.getDelegate();
        if(componentType instanceof GenericClass){
            int length = info.elementCount();
            return new GenericArray(((GenericClass)componentType).arrayClass(), length);
        }
        return _delegate.newInstance(componentType, info);
    }

    public Object newInstance(ReflectClass componentType, int length) {
        componentType = componentType.getDelegate();
        if(componentType instanceof GenericClass){
            return new GenericArray(((GenericClass)componentType).arrayClass(), length);
        }
        return _delegate.newInstance(componentType, length);
    }

    public Object newInstance(ReflectClass componentType, int[] dimensions) {
        return _delegate.newInstance(componentType.getDelegate(), dimensions);
    }

    public void set(Object onArray, int index, Object element) {
        if(onArray instanceof GenericArray){
            ((GenericArray)onArray)._data[index] = element;
            return;
        }
        _delegate.set(onArray, index, element);
    }

    public int shape(Object[] a_flat, int a_flatElement, Object a_shaped, int[] a_dimensions, int a_currentDimension) {
        return _delegate.shape(a_flat, a_flatElement, a_shaped, a_dimensions, a_currentDimension);
    }


}
