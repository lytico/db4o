package com.db4o.rmi;

import java.util.*;

public class ClassResolver {
	
	static final Map<String, Class<?>> builtInMap = new HashMap<String, Class<?>>();
	
	static {
		builtInMap.put("int", Integer.TYPE);
		builtInMap.put("long", Long.TYPE);
		builtInMap.put("double", Double.TYPE);
		builtInMap.put("float", Float.TYPE);
		builtInMap.put("boolean", Boolean.TYPE);
		builtInMap.put("char", Character.TYPE);
		builtInMap.put("byte", Byte.TYPE);
		builtInMap.put("void", Void.TYPE);
		builtInMap.put("short", Short.TYPE);
	}

	public static Class<?> forName(String className) throws ClassNotFoundException {
		Class<?> clazz = builtInMap.get(className);
		if (clazz != null) {
			return clazz;
		}
		return Class.forName(className);
	}

}
