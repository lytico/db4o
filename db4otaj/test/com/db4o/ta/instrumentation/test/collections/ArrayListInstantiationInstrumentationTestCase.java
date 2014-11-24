package com.db4o.ta.instrumentation.test.collections;

import java.util.*;

import com.db4o.collections.*;
import com.db4o.internal.*;
import com.db4o.ta.instrumentation.*;
import com.db4o.ta.instrumentation.test.*;

import db4ounit.*;

public class ArrayListInstantiationInstrumentationTestCase implements TestCase {

	public void testConstructorIsExchanged() throws Exception {
		Class instrumented = instrument(ArrayListFactory.class);
		Object instance = instrumented.newInstance();
		assertReturnsActivatableList(instance, "createArrayList");
		assertReturnsActivatableList(instance, "createSizedArrayList");
		assertReturnsActivatableList(instance, "createNestedArrayList");
		assertReturnsActivatableList(instance, "createMethodArgArrayList");
		assertReturnsActivatableList(instance, "createConditionalArrayList");
	}

	public void testBaseTypeIsExchanged() throws Exception {
		Class instrumented = instrument(MyArrayList.class);
		List list = (List)instrumented.newInstance();
		assertActivatableList(list);
		List delegateList = (List)instrumented.getField("_delegate").get(list);
		assertActivatableList(delegateList);
	}
	
	public void testBaseInvocationIsExchanged() throws Exception {
		Class instrumented = instrument(MyArrayList.class);
		List list = (List)instrumented.newInstance();
		list.add("foo");
		Assert.isTrue(list.contains("foo"));
		
		List delegateList = (List)instrumented.getField("_delegate").get(list);
		Assert.isTrue(delegateList.contains("foo"));
	}
	
	private void assertActivatableList(List delegateList) {
	    Assert.isInstanceOf(ActivatableArrayList.class, delegateList);
    }
	
	private void assertReturnsActivatableList(Object instance, String methodName) {
		List list = invokeForListCreation(instance, methodName);
		assertActivatableList(list);
	}

	private List invokeForListCreation(Object instance, String methodName) {
		return (List)Reflection4.invoke(instance, methodName);
	}

	private Class instrument(Class clazz) throws ClassNotFoundException {
		return InstrumentationEnvironment.enhance(clazz, new ReplaceClassOnInstantiationEdit(ArrayList.class, ActivatableArrayList.class));
	}
}
