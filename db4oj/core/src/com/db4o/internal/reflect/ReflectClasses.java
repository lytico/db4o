package com.db4o.internal.reflect;

import com.db4o.reflect.*;

public class ReflectClasses {

	public static boolean areEqual(Class expected, ReflectClass actual) {
		return actual.reflector().forClass(expected) == actual;
	}
}
