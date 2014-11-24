/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.soda.arrays;

import com.db4o.db4ounit.common.soda.arrays.object.*;
import com.db4o.db4ounit.common.soda.arrays.typed.*;
import com.db4o.db4ounit.common.soda.arrays.untyped.*;

import db4ounit.extensions.*;


public class AllTests extends Db4oTestSuite {

	protected Class[] testCases() {
		return new Class[] {
				STArrMixedTestCase.class,
				STArrStringOTestCase.class,
				STArrStringONTestCase.class,
				STArrStringTTestCase.class,
				STArrStringTNTestCase.class,
				STArrStringUTestCase.class,
				STArrStringUNTestCase.class,
				STArrIntegerOTestCase.class,
				STArrIntegerONTestCase.class,
				STArrIntegerTTestCase.class,
				STArrIntegerTNTestCase.class,
				STArrIntegerUTestCase.class,
				STArrIntegerUNTestCase.class,
				STArrIntegerWTTestCase.class,
				STArrIntegerWTONTestCase.class,
				STArrIntegerWUONTestCase.class,
				ArrayDescendSubQueryTestCase.class,
		};
	}
}
