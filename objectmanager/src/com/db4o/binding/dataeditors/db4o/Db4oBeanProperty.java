/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.binding.dataeditors.db4o;

import java.lang.reflect.InvocationHandler;
import java.lang.reflect.Method;
import java.lang.reflect.Proxy;

import org.eclipse.ve.sweet.objectviewer.IPropertyEditor;

import com.db4o.ObjectContainer;
import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectField;
import com.db4o.reflect.ReflectMethod;
import com.db4o.reflect.Reflector;

/**
 * Db4oBeanProperty. An implementation of IPropertyEditor using dynamic proxies.
 * 
 * @author djo
 */
public class Db4oBeanProperty implements InvocationHandler {

    public static IPropertyEditor construct(Object receiver, String propertyName, ObjectContainer database) throws NoSuchMethodException {
        try {
            return (IPropertyEditor) Proxy.newProxyInstance(Db4oBeanProperty.class.getClassLoader(),
                    new Class[] { IPropertyEditor.class }, new Db4oBeanProperty(
                            receiver, propertyName, database));
        } catch (IllegalArgumentException e) {
            throw new NoSuchMethodException(e.getMessage());
        }
    }

    private String propertyName;
    private ReflectClass propertyType;
    private Object receiver;
    private ObjectContainer database;

    private ReflectClass receiverClass;
    
    private ReflectMethod setter = null;
    private ReflectMethod getter;
    private ReflectField field;

    /**
     * Construct a JavaBeansProperty object on the specified object and property
     * 
     * @param receiver
     * @param propertyName
     */
    private Db4oBeanProperty(Object receiver, String propertyName, ObjectContainer database)
            throws NoSuchMethodException {
        this.receiver = receiver;
        this.database = database;
        this.receiverClass = database.ext().reflector().forObject(receiver);
        this.propertyName = propertyName;

        // There must be at least a getter or a field...
        getter = receiverClass.getMethod(realMethodName("get"), noParams);
        if (getter != null) {
            propertyType = getter.getReturnType();
        } else {
            field = getField(receiverClass, propertyName);
            if (field == null) {
                field = getField(receiverClass, lowerCaseFirstLetter(propertyName));
                if (field == null) {
                    throw new NoSuchMethodException("That property does not exist.");
                }
            }
            field.setAccessible();
            propertyType = field.getFieldType();
        }
        
        setter = receiverClass.getMethod(
                realMethodName("set"), new ReflectClass[] {propertyType});
    }
    
    private ReflectField getField(ReflectClass clazz, String propertyName) {
        ReflectField result;
        while (clazz != null) {
            result = clazz.getDeclaredField(propertyName);
            if (result != null)
                return result;
            clazz = clazz.getSuperclass();
        }
        return null;
    }

    private String lowerCaseFirstLetter(String name) {
    	if (name.length() >= 1) {
	        String result = name.substring(0, 1).toLowerCase() + name.substring(1);
	        return result;
    	}
    	return name;
    }

    private Object get() {
        if (getter != null) {
            return getter.invoke(receiver, new Object[] {});
        }
        if (field != null)
            return field.get(receiver);
        else
            return null;
    }
    
    private void set(Object[] args) {
        if (setter != null) {
            setter.invoke(receiver, args);
            return;
        }
        if (field != null) {
            field.set(receiver, args[0]);
        }
    }
    
    /*
     * This implements a semi-relaxed duck-type over IPropertyEditor. The
     * required method is get<propertyName>. getType, getInput, and setInput
     * are implemented internally.
     */
    public Object invoke(Object proxy, Method method, Object[] args)
            throws Throwable {
        if ("set".equals(method.getName())) {
            set(args);
            return null;
        } else if ("get".equals(method.getName())) {
            return get();
        } else if ("getType".equals(method.getName())) {
            return propertyType.getName();
        } else if ("getInput".equals(method.getName())) {
            return receiver;
        } else if ("setInput".equals(method.getName())) {
            this.receiver = args[0];
        } else if ("isReadOnly".equals(method.getName())) {
            return new Boolean(setter == null && field == null);
        } else if ("getName".equals(method.getName())) {
            return propertyName;
        } 

        ReflectMethod realMethod;
        realMethod = receiverClass.getMethod(
                realMethodName(method.getName()), toReflectClass(method.getParameterTypes()));
        if (realMethod == null) {
            return null;
        }
        return realMethod.invoke(receiver, args);
    }

    private ReflectClass[] toReflectClass(Class[] parameterTypes) {
        ReflectClass[] results = new ReflectClass[parameterTypes.length];
        Reflector reflector = database.ext().reflector();
        for (int i = 0; i < results.length; i++) {
            results[i] = reflector.forClass(parameterTypes[i]);
        }
        return results;
    }

    String realMethodName(String interfaceMethodName) {
        return interfaceMethodName.substring(0, 3) + propertyName
                + interfaceMethodName.substring(3);
    }

    private static final ReflectClass[] noParams = new ReflectClass[] {};

}
