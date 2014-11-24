/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.internal;

import java.util.*;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.metadata.*;
import com.db4o.internal.metadata.HierarchyAnalyzer.*;
import com.db4o.reflect.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class HierarchyAnalyzerTestCase extends AbstractDb4oTestCase{
	
	public static class A {
		
	}
	
	public static class BA extends A {
		
	}
	
	public static class CBA extends BA {
		
	}
	
	public static class DA extends A {
		
	}
	
	public static class E {
		
	}
	
	
	public void testRemovedImmediateSuperclass(){
		assertDiffBetween(DA.class, CBA.class,
				new Removed(produceClassMetadata(BA.class)),
				new Same(produceClassMetadata(A.class)));
	}
	
	public void testRemoveTopLevelSuperclass(){
		assertDiffBetween(E.class, BA.class,
				new Removed(produceClassMetadata(A.class)));
	}
	
	public void testAddedImmediateSuperClass(){
		Assert.expect(IllegalStateException.class, new CodeBlock() {
			public void run() throws Throwable {
				assertDiffBetween(CBA.class, DA.class);
			}
		});
	}
	
	public void testAddedTopLevelSuperClass(){
		Assert.expect(IllegalStateException.class, new CodeBlock() {
			public void run() throws Throwable {
				assertDiffBetween(BA.class, E.class);
			}
		});
	}


	private void assertDiffBetween(Class<?> runtimeClass, Class<?> storedClass,
			Diff... expectedDiff) {
		ClassMetadata classMetadata = produceClassMetadata(storedClass);
		ReflectClass reflectClass = reflectClass(runtimeClass);
		List<Diff> ancestors = new HierarchyAnalyzer(classMetadata, reflectClass).analyze();
		assertDiff(ancestors, expectedDiff );
	}


	private ClassMetadata produceClassMetadata(Class<?> storedClass) {
		return container().produceClassMetadata(reflectClass(storedClass));
	}


	private void assertDiff(List<Diff> actual, Diff...expected) {
		Iterator4Assert.areEqual(Iterators.iterate(expected), Iterators.iterator(actual) );
	}

}
