/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;


public class Stack4TestCase implements TestCase {

	public static void main(String[] args) {
		new ConsoleTestRunner(Stack4TestCase.class).run(); 
	}
	
	public void testPushPop(){
		final Stack4 stack = new Stack4();
		assertEmpty(stack);
		stack.push("a");
		stack.push("b");
		stack.push("c");
		Assert.isFalse(stack.isEmpty());
		Assert.areEqual("c", stack.peek());
		Assert.areEqual("c", stack.peek());
		Assert.areEqual("c", stack.pop());
		Assert.areEqual("b", stack.pop());
		Assert.areEqual("a", stack.peek());
		Assert.areEqual("a", stack.pop());
		assertEmpty(stack);
	}

	private void assertEmpty(final Stack4 stack) {
		Assert.isTrue(stack.isEmpty());
		Assert.isNull(stack.peek());
		Assert.expect(IllegalStateException.class, new CodeBlock() {
			public void run() throws Throwable {
				stack.pop();
			}
		});
	}

}
