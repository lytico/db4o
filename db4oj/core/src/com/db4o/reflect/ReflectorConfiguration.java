package com.db4o.reflect;

public interface ReflectorConfiguration {

	boolean testConstructors();
	
	boolean callConstructor(ReflectClass clazz);

}
