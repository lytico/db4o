/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test.nativequery;

import com.db4o.foundation.*;
import com.db4o.test.nativequery.analysis.*;
import com.db4o.test.nativequery.cats.*;
import com.db4o.test.nativequery.diagnostics.*;
import com.db4o.test.nativequery.expr.*;
import com.db4o.test.nativequery.expr.build.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class AllTestsNQ {
	
	public static void main(String[] args) {
		Iterable4 plainTests=new ReflectionTestSuiteBuilder(
				new Class[] {
						ExpressionBuilderTestCase.class,
						BloatExprBuilderVisitorTestCase.class,
						ExpressionTestCase.class,
						BooleanReturnValueTestCase.class,
						NQBuildTimeInstrumentationTestCase.class,
				}
		);
		Iterable4 db4oTests=new Db4oTestSuiteBuilder(new Db4oSolo(),
					new Class[] {
						NativeQueryOptimizerDiagnosticsTestCase.class,
						NQDateCompareToTestCase.class,
						NQGreaterOrEqualTestCase.class,
						NQRegressionTestCase.class,
						NQCatConsistencyTestCase.class,
						NQUnoptimizableCollectionMethodTestCase.class,
						NQPredicateFalseTestCase.class,
					}
		);
		Iterable4 allTests=Iterators.concat(
				new Iterable4[] {
					plainTests,
					db4oTests,
				}
		);
		System.exit(new ConsoleTestRunner(allTests).run());
	}
}
