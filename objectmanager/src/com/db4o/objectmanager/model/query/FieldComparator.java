/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.objectmanager.model.query;

import java.util.Comparator;

import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;

public class FieldComparator implements Comparator {
    public int compare(Object first, Object second) {
        ReflectField firstField=(ReflectField)first;
        ReflectField secondField=(ReflectField)second;
        int fieldNameComp=firstField.getName().compareTo(secondField.getName());
        if(fieldNameComp!=0) {
            return fieldNameComp;
        }
        ReflectClass firstType=firstField.getFieldType();
        ReflectClass secondType=secondField.getFieldType();
        int typeNameComp=firstType.getName().compareTo(secondType.getName());
        if(typeNameComp!=0) {
            return typeNameComp;
        }
        boolean firstPrimitive=firstType.isPrimitive();
        boolean secondPrimitive=secondType.isPrimitive();
        return (firstPrimitive==secondPrimitive ? 0 : (firstPrimitive ? -1 : 1));
    }
}