package com.db4o.reflect.core;

import com.db4o.reflect.*;

public interface ConstructorAwareReflectClass extends ReflectClass {

	public ReflectConstructor getSerializableConstructor();
    
}
