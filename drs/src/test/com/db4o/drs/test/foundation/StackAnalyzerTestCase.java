/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.foundation;

import com.db4o.drs.foundation.*;

import db4ounit.*;

/**
 * @sharpen.remove
 */
public class StackAnalyzerTestCase implements TestCase {
	
	public void test1(){
		Assert.areEqual("StackAnalyzerTestCase#test1()", StackAnalyzer.methodCallAsString(1));
	}
	
	public void test2(){
		assert2();
	}

	private void assert2() {
		Assert.areEqual("StackAnalyzerTestCase#assert2()", StackAnalyzer.methodCallAsString(1));		
		Assert.areEqual("StackAnalyzerTestCase#test2()", StackAnalyzer.methodCallAsString(2));
	}

}
