/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.qlin;

import java.lang.reflect.*;

import com.db4o.foundation.*;
import com.db4o.qlin.*;

import db4ounit.*;

/**
 * @sharpen.if !SILVERLIGHT
 */
@decaf.Remove(decaf.Platform.JDK11)
public class PuzzleTypesafeFieldObject implements TestCase{
	
	private static Prototypes _prototypes = new Prototypes(); 
	
	public static class Cat {
		
		public String name;

		public Cat(String name){
			this.name = name;
		}
	}
	
	public void testTypeSafeFieldAsObject() {
		Cat cat = prototype(Cat.class);
		Field nameField = field(cat, cat.name);
	}
	
	private <T> T prototype(Class<T> clazz) {
		return _prototypes.prototypeForClass(clazz);
	}
	
	public static Field field(Object onObject, Object expression) {
		Class clazz = onObject.getClass();
		Iterator4<String> path = _prototypes.backingFieldPath(onObject.getClass(), expression);
		path.moveNext();
		System.out.println(path.current());
		return null;
	}
	
	public void setUp() throws Exception {
		_prototypes = new Prototypes(Prototypes.defaultReflector(), RECURSION_DEPTH, IGNORE_TRANSIENT_FIELDS);
	}

	public void tearDown() throws Exception {
		
	}
	
	private static final boolean IGNORE_TRANSIENT_FIELDS = true;
	
	private static final int RECURSION_DEPTH = 10;
	
}
