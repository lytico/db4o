package com.db4o.test;

import java.util.*;

import com.db4o.reflect.self.*;

/* GENERATE */
public class RegressionDogSelfReflectionRegistry extends SelfReflectionRegistry {
	private final static Hashtable CLASSINFO;

	static {
		CLASSINFO = new Hashtable(2);
		CLASSINFO.put(Animal.class, new ClassInfo(true, Object.class,
				new FieldInfo[] { new FieldInfo("_name", String.class, true,
						false, false) }));
		CLASSINFO.put(Dog.class,
				new ClassInfo(false, Animal.class,
						new FieldInfo[] {
								new FieldInfo("_age", Integer.class, true,
										false, false),
								new FieldInfo("_parents", Dog[].class, true,
										false, false), 
								new FieldInfo("_prices", int[].class, true,
										false, false),
				}));
		// FIELDINFO.put(P1Object.class, new FieldInfo[]{});
	}

	public ClassInfo infoFor(Class clazz) {
		return (ClassInfo) CLASSINFO.get(clazz);
	}

	public Object arrayFor(Class clazz, int length) {
		if (Dog.class.isAssignableFrom(clazz)) {
			return new Dog[length];
		}
		if (Animal.class.isAssignableFrom(clazz)) {
			return new Animal[length];
		}
		return super.arrayFor(clazz, length);
	}

	public Class componentType(Class clazz) {
		if (Dog[].class.isAssignableFrom(clazz)) {
			return Dog.class;
		}
		if (Animal[].class.isAssignableFrom(clazz)) {
			return Animal.class;
		}
		return super.componentType(clazz);
	}
}
