package com.db4o.reflect.core;

import com.db4o.internal.*;
import com.db4o.reflect.*;

public class PlatformReflectConstructor implements ReflectConstructor {

	private static final ReflectClass[] PARAMETER_TYPES = new ReflectClass[]{};

	private Class _clazz;
	
	public PlatformReflectConstructor(Class clazz) {
		_clazz = clazz;
	}
	
	public ReflectClass[] getParameterTypes() {
		return PARAMETER_TYPES;
	}

	public Object newInstance(Object[] parameters) {
		return ReflectPlatform.createInstance(_clazz);
	}

}
