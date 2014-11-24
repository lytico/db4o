/* Copyright (C) 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.qlin;

import com.db4o.foundation.*;
import com.db4o.qlin.*;

import db4ounit.*;

/**
 * @sharpen.if !SILVERLIGHT
 */
@decaf.Remove(decaf.Platform.JDK11)
public class PrototypesTestCase implements TestLifeCycle {
	
	private Prototypes _prototypes; 
	
	public static class Item {
		
		
		public Item _child;
		
		public String _name;
		
		public int myInt;
		
		public String name(){
			return _name;
		}
		
		public Item child(){
			return _child;
		}
		
		@Override
		public String toString() {
			String str = "Item " + _name;
			if(_child != null){
				str += "\n  " + _child.toString();
			}
			return str;
		}
		
	}
	
	
	public void testStringField(){
		Item item = prototype(Item.class);
		assertPath(item, item._name, "_name");
	}
	
	public void testStringMethod(){
		Item item = prototype(Item.class);
		assertPath(item, item.name(), "_name");
	}
	
	public void testInstanceField(){
		Item item = prototype(Item.class);
		assertPath(item, item._child, "_child");
	}
	
	public void testInstanceMethod(){
		Item item = prototype(Item.class);
		assertPath(item, item.child(), "_child");
	}
	
	public void testLevel2(){
		Item item = prototype(Item.class);
		assertPath(item, item.child().name(), "_child", "_name");
	}
	
	public void testCallingOwnFramework(){
		PrototypesTestCase testCase = prototype(PrototypesTestCase.class);
		assertPath(testCase, testCase._prototypes, "_prototypes");
	}
	
	public void testWildToString(){
		PrototypesTestCase testCase = prototype(PrototypesTestCase.class);
		assertIsNull(testCase, testCase._prototypes.toString());
	}
	
	
	// keep this method, it's helpful for new tests
	private <T> void print(T t, Object expression){
		Iterator4<String> path = _prototypes.backingFieldPath(t.getClass(), expression);
		if(path == null){
			print("null");
			return;
		}
		print(Iterators.join(path, "[", "]", ", "));
	}

	private void print(String string) {
		System.out.println(string);
	}
	
	private <T> void assertIsNull(T t, Object expression) {
		Assert.isNull(_prototypes.backingFieldPath(t.getClass(), expression));
	}

	private <T> void assertPath(T t, Object expression, String... expected) {
		Iterator4<String> path = _prototypes.backingFieldPath(t.getClass(), expression);
		// print(Iterators.join(path, "[", "]", ", "));
		path.reset();
		Iterator4Assert.areEqual(expected, path);
	}

	private <T> T prototype(Class<T> clazz) {
		return _prototypes.prototypeForClass(clazz);
	}


	public void setUp() throws Exception {
		_prototypes = new Prototypes(Prototypes.defaultReflector(), RECURSION_DEPTH, IGNORE_TRANSIENT_FIELDS);
	}

	public void tearDown() throws Exception {
		
	}
	
	private static final boolean IGNORE_TRANSIENT_FIELDS = true;
	
	private static final int RECURSION_DEPTH = 10;
	
}
