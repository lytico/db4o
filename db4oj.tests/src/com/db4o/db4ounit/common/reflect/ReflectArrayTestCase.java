/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.reflect;

import com.db4o.reflect.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ReflectArrayTestCase extends AbstractDb4oTestCase {
	
	public void testNewInstance() {
		String[][] a23 = newStringMatrix(2, 3);
		Assert.areEqual(2, a23.length);
		for (int i=0; i<a23.length; ++i) {
			Assert.areEqual(3, a23[i].length);
		}
	}

	private String[][] newStringMatrix(final int x, final int y) {
		return (String[][])newInstance(String.class, new int[] { x, y });
	}
	
	public void testIsNDimensional() {
		ReflectClass arrayOfArrayOfString = reflectClass(String[][].class);
		Assert.isTrue(arrayOfArrayOfString.isArray());
		
		final ReflectClass arrayOfString = reflectClass(String[].class);
		Assert.areSame(arrayOfString, arrayOfArrayOfString.getComponentType());
		Assert.isTrue(arrayReflector().isNDimensional(arrayOfArrayOfString));		
		Assert.isFalse(arrayReflector().isNDimensional(arrayOfString));
	}

	public void testDimensions() {
		String[][] array = newStringMatrix(3, 4);
		ArrayAssert.areEqual(new int[] { 3, 4 }, arrayReflector().dimensions(array));
	}
	
	private Object newInstance(Class elementType, int[] dimensions) {
		return arrayReflector().newInstance(reflectClass(elementType), dimensions);
	}

	private ReflectArray arrayReflector() {
		return reflector().array();
	}

}
