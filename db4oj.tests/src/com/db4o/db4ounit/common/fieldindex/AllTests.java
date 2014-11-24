/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.fieldindex;


import db4ounit.extensions.*;

public class AllTests extends ComposibleTestSuite {

	public static void main(String[] args) {
		new AllTests().runSolo();
    }

	protected Class[] testCases() {
		return composeTests(new Class[] {
				IndexedNodeTestCase.class,
	            FieldIndexTestCase.class,
	            FieldIndexProcessorTestCase.class,
	            StringFieldIndexTestCase.class,
				DoubleFieldIndexTestCase.class,
				RuntimeFieldIndexTestCase.class,
				SecondLevelIndexTestCase.class,
				StringFieldIndexDefragmentTestCase.class,
	            StringIndexTestCase.class,
	            StringIndexCorruptionTestCase.class,
	            StringIndexWithSuperClassTestCase.class,
	            UseSecondBestIndexTestCase.class,
		});
    }
	
	/**
	 * @sharpen.if !SILVERLIGHT
	 */
	@Override
	protected Class[] composeWith() {
		return new Class[] {
				CommitAfterDroppedFieldIndexTestCase.class,
				};
	}

}
