/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
package com.db4o.ta.instrumentation.test.collections;

import java.util.*;

import com.db4o.collections.*;
import com.db4o.internal.*;
import com.db4o.ta.instrumentation.*;
import com.db4o.ta.instrumentation.test.*;

import db4ounit.*;

@SuppressWarnings("unchecked")
public class StackInstantiationInstrumentationTestCase implements TestCase {
	public static void main(String[] args) {
		new ConsoleTestRunner(StackInstantiationInstrumentationTestCase.class).run();
	}
	
	public void testConstructorIsExchanged() throws Exception {
		Class instrumented = instrument(StackFactory.class);
		Object instance = instrumented.newInstance();
		assertReturnsActivatableStack(instance, "createStack");
	}

	public void testBaseTypeIsExchanged() throws Exception {
		Class instrumented = instrument(MyStack.class);
		Stack stack = (Stack)instrumented.newInstance();
		assertActivatableStack(stack);
		Stack delegateStack = (Stack)instrumented.getField("_delegate").get(stack);
		assertActivatableStack(delegateStack);
	}
	
	public void testBaseInvocationIsExchanged() throws Exception {
		Class instrumented = instrument(MyStack.class);
		Stack stack = (Stack)instrumented.newInstance();
		stack.push("foo");
		Assert.areEqual("foo", stack.peek());
		
		Stack delegateStack = (Stack)instrumented.getField("_delegate").get(stack);
		Assert.areEqual("foo", delegateStack.peek());
	}
	
	private void assertActivatableStack(Stack delegateStack) {
	    Assert.isInstanceOf(ActivatableStack.class, delegateStack);
    }
	
	private void assertReturnsActivatableStack(Object instance, String methodName) {
		Stack stack = invokeForStackCreation(instance, methodName);
		assertActivatableStack(stack);
	}

	private Stack invokeForStackCreation(Object instance, String methodName) {
		return (Stack) Reflection4.invoke(instance, methodName);
	}

	private Class instrument(Class clazz) throws ClassNotFoundException {
		return InstrumentationEnvironment.enhance(clazz, new ReplaceClassOnInstantiationEdit(Stack.class, ActivatableStack.class));
	}

}
