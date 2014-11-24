package com.db4o.objectmanager;

import db4ounit.TestCase;
import db4ounit.TestLifeCycle;
import com.db4o.ObjectContainer;

import java.lang.reflect.Array;

/**
 * User: treeder
 * Date: Dec 18, 2006
 * Time: 11:35:46 PM
 */
public class PrimitivesTest implements TestCase, TestLifeCycle {
	public static void main(String[] args) {
		PrimitivesTest test = new PrimitivesTest();
		test.testArrayCasting();
	}
	public void setUp() throws Exception {

	}

	public void tearDown() throws Exception {

	}

	public void testArrayCasting(){
		boolean[] booleans = new boolean[10];
		booleans[1] = true;
		Object bob = booleans;
		//Object[] x = (Object[]) bob; fails
		System.out.println("isArray? " + bob.getClass().isArray());
		System.out.println("isPrimitive? " + bob.getClass().isPrimitive());
		Class objectsType = bob.getClass().getComponentType();
		System.out.println("objectsType: " + objectsType);
		System.out.println("equalToBooleanType? " + (objectsType == Boolean.TYPE));
		if(bob instanceof boolean[]){
			System.out.println("casting ok");
		}
		System.out.println("value at 0: " + Array.get(bob, 0));
		System.out.println("value at 1: " + Array.get(bob, 1));
		System.out.println("length: " + Array.getLength(bob));
		
	}

}

