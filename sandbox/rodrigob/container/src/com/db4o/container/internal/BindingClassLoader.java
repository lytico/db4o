/**
 * 
 */
package com.db4o.container.internal;

final class BindingClassLoader extends ClassLoader {

	public BindingClassLoader(ClassLoader parent) {
		super(parent);
	}

	public Class define(String className, byte[] classBytes) {
		return defineClass(className, classBytes, 0, classBytes.length);
	}
}