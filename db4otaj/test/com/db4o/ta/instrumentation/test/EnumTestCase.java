/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.ta.instrumentation.test;

import java.lang.reflect.*;
import java.net.*;

import com.db4o.instrumentation.classfilter.*;
import com.db4o.instrumentation.core.*;
import com.db4o.instrumentation.main.*;
import com.db4o.ta.*;
import com.db4o.ta.instrumentation.*;

import db4ounit.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class EnumTestCase implements TestLifeCycle{
	
	private BloatInstrumentingClassLoader _loader;
	
	public static enum MyEnum {
		
		FOO("foo");
		
		public String name;

		MyEnum(String name) {
			this.name = name;
		}
		
		@Override
		public String toString() {
			return name;
		}
	}
	
	public static class MyEnumClient {
		
		public static String nameOf(MyEnum e) {
			return e.name;
		}
		
	}
	
	public void setUp() throws Exception {
		ClassLoader baseLoader = MyEnum.class.getClassLoader();
		ClassFilter filter = new ByNameClassFilter(new String[]{ enumClassName(), enumClientClassName(), });
		_loader = new BloatInstrumentingClassLoader(new URL[] {}, baseLoader, new AcceptAllClassesFilter(), new InjectTransparentActivationEdit(filter));
	}

	private String enumClientClassName() {
	    return MyEnumClient.class.getName();
    }

	private String enumClassName() {
		return MyEnum.class.getName();
	}
	
	public void testEnumIsNotActivatable() throws Exception {
		Class<?> enumClass = _loader.loadClass(enumClassName());
		Assert.isFalse(Activatable.class.isAssignableFrom(enumClass));
		Assert.areEqual("foo", fooEnumFrom(enumClass).toString());
	}
	
	public void testEnumFieldAccessIsNotEnhanced() throws Exception {
		final Class<?> enumClientClass = _loader.loadClass(enumClientClassName());
		final Class<?> enumClass = _loader.loadClass(enumClassName());
		final Method nameOfMethod = enumClientClass.getMethod("nameOf", enumClass);
		Assert.areEqual("foo", nameOfMethod.invoke(null, fooEnumFrom(enumClass)));
	}

	private Object fooEnumFrom(Class<?> enumClass) throws IllegalAccessException, NoSuchFieldException {
	    return enumClass.getField("FOO").get(null);
    }

	public void tearDown() throws Exception {
		
	}


}
