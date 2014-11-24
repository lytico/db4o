/* Copyright (C) 2009  db4objects Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation.test.collections;

import java.util.*;

import com.db4o.collections.*;
import com.db4o.internal.*;
import com.db4o.ta.instrumentation.*;
import com.db4o.ta.instrumentation.test.*;

import db4ounit.*;

public class TreeSetInstantiationInstrumentationTestCase implements TestCase {

	public void testConstructorIsExchanged() throws Exception {
		Class instrumented = instrument(TreeSetFactory.class);
		Object instance = instrumented.newInstance();
		assertReturnsActivatableSet(instance, "createTreeSet");
		assertReturnsActivatableSet(instance, "createTreeSetWithComparator");
		assertReturnsActivatableSet(instance, "createTreeSetFromCollection");
		assertReturnsActivatableSet(instance, "createTreeSetFromSortedSet");
	}
	
	public void testBaseTypeIsExchanged() throws Exception {
		Class instrumented = instrument(MyTreeSet.class);
		Set set = (Set)instrumented.newInstance();
		assertActivatableSet(set);
		Set delegateSet = (Set)instrumented.getField("_delegate").get(set);
		assertActivatableSet(delegateSet);
	}

	public void testBaseInvocationIsExchanged() throws Exception {
		Class instrumented = instrument(MyTreeSet.class);
		Set set = (Set)instrumented.newInstance();
		set.add("foo");
		Assert.isTrue(set.contains("foo"));
		
		Set delegateSet = (Set)instrumented.getField("_delegate").get(set);
		Assert.isTrue(delegateSet.contains("foo"));
	}
	
	private void assertActivatableSet(Set delegateSet) {
	    Assert.isInstanceOf(ActivatableTreeSet.class, delegateSet);
    }
	
	private void assertReturnsActivatableSet(Object instance, String methodName) {
		Set set = (Set)Reflection4.invoke(instance, methodName);
		assertActivatableSet(set);
	}

	private Class instrument(Class clazz) throws ClassNotFoundException {
		return InstrumentationEnvironment.enhance(clazz, new ReplaceClassOnInstantiationEdit(TreeSet.class, ActivatableTreeSet.class));
	}
}
