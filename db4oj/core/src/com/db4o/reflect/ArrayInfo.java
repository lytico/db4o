/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.reflect;


/**
 * @exclude
 */
public class ArrayInfo {
    
    private int _elementCount;
    
    private boolean _primitive;
    
    private boolean _nullable;
    
    private ReflectClass _reflectClass;
    
    public int elementCount() {
        return _elementCount;
    }
    
    public void elementCount(int count) {
        _elementCount = count;
    }
    
    public boolean primitive() {
        return _primitive;
    }
    
    public void primitive(boolean flag) {
        _primitive = flag;
    }
    
    public boolean nullable() {
        return _nullable;
    }
    
    public void nullable(boolean flag) {
        _nullable = flag;
    }
    
    public ReflectClass reflectClass() {
        return _reflectClass;
    }
    
    public void reflectClass(ReflectClass claxx) {
        _reflectClass = claxx;
    }
    
}
