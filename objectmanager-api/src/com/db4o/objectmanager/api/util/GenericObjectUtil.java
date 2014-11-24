package com.db4o.objectmanager.api.util;

import com.db4o.reflect.generic.GenericClass;
import com.db4o.reflect.generic.GenericReflector;
import com.db4o.reflect.jdk.JdkReflector;

/**
 * User: treeder
 * Date: Mar 19, 2007
 * Time: 1:46:21 AM
 */
public class GenericObjectUtil {
	public static GenericClass makeGenericClass(String className) {
		GenericReflector reflector = getReflector();
		GenericClass _objectIClass = (GenericClass) reflector
				.forClass(Object.class);
		GenericClass result = new GenericClass(reflector, null, className, _objectIClass);
		return result;
	}

	public static GenericReflector getReflector() {
		GenericReflector reflector = new GenericReflector(null,
				new JdkReflector(Thread.currentThread().getContextClassLoader()));
		return reflector;
	}

}
