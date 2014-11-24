/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.reflect.self;

import com.db4o.reflect.*;

public class SelfReflector implements Reflector {
	private SelfArray _arrayHandler;
	private SelfReflectionRegistry _registry;

	private Reflector _parent;

	public SelfReflector(SelfReflectionRegistry registry) {
		_registry = registry;
	}

	public ReflectArray array() {
		if(_arrayHandler==null) {
			_arrayHandler=new SelfArray(this,_registry);
		}
		return _arrayHandler;
	}

	public boolean constructorCallsSupported() {
		return true;
	}

	public ReflectClass forClass(Class clazz) {
		return new SelfClass(_parent, _registry, clazz);
	}

	public ReflectClass forName(String className) {
		try {
			Class clazz = Class.forName(className);
			return forClass(clazz);
		} catch (ClassNotFoundException e) {
			return null;
		}
	}

	public ReflectClass forObject(Object a_object) {
		if (a_object == null) {
			return null;
		}
		return _parent.forClass(a_object.getClass());
	}

	public boolean isCollection(ReflectClass claxx) {
		return false;
	}

	public void setParent(Reflector reflector) {
		_parent = reflector;
	}

	public Object deepClone(Object context) {
		return new SelfReflector(_registry);
	}
}
