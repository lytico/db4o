package com.db4o.drs.versant.jdo.reflect;

import com.db4o.reflect.*;

public class NotImplementedReflector implements Reflector {

	public Object deepClone(Object context) {
		throw new java.lang.UnsupportedOperationException();
	}

	public void configuration(ReflectorConfiguration config) {
		throw new java.lang.UnsupportedOperationException();
	}

	public ReflectArray array() {
		throw new java.lang.UnsupportedOperationException();
	}

	public ReflectClass forClass(Class clazz) {
		throw new java.lang.UnsupportedOperationException();
	}

	public ReflectClass forName(String className) {
		throw new java.lang.UnsupportedOperationException();
	}

	public ReflectClass forObject(Object obj) {
		throw new java.lang.UnsupportedOperationException();
	}

	public boolean isCollection(ReflectClass clazz) {
		throw new java.lang.UnsupportedOperationException();
	}

	public void setParent(Reflector reflector) {
		throw new java.lang.UnsupportedOperationException();
	}

}
