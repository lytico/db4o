package com.db4o.reflect.self;

import com.db4o.reflect.ReflectClass;
import com.db4o.reflect.ReflectConstructor;

public class SelfConstructor implements ReflectConstructor{

	private Class _class;
	
	public SelfConstructor(Class _class) {
		this._class = _class;
	}

	public void setAccessible() {
	}
	
	public ReflectClass[] getParameterTypes() {
		return new ReflectClass[] {};
	}

	public Object newInstance(Object[] parameters) {
		try {
			return _class.newInstance();
		} catch (Exception exc) {
			return null;
		}
	}

}
