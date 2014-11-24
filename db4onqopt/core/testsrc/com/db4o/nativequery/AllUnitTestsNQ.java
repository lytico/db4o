/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.nativequery;

import com.db4o.nativequery.analysis.*;
import com.db4o.nativequery.expr.*;
import com.db4o.nativequery.expr.build.*;

import db4ounit.*;

public class AllUnitTestsNQ {
	
	public static void main(String[] args) {
		int ret=new TestRunner(build()).run();
		if(ret!=0) {
			throw new RuntimeException(ret+" tests failed");
		}
	}

	public static TestSuite build() {
		return new ReflectionTestSuiteBuilder(
				new Class[] {
						ExpressionUnitTest.class,
						ExpressionBuilderUnitTest.class,
						BloatExprBuilderVisitorUnitTest.class
					}).build();
	}
}
