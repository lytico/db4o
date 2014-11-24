/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre11.soda;

import com.db4o.db4ounit.jre11.soda.classes.simple.*;
import com.db4o.db4ounit.jre11.soda.wrapper.typed.*;
import com.db4o.db4ounit.jre11.soda.wrapper.untyped.*;

import db4ounit.extensions.*;


public class AllTests extends Db4oTestSuite {

	protected Class[] testCases() {
		return new Class[] {
			SodaNumberCoercionTestCase.class,
			STBooleanWTTestCase.class,
			STByteWTTestCase.class,
			STCharWTTestCase.class,
			STDateTestCase.class,
			STDateUTestCase.class,
			STDoubleWTTestCase.class,
			STFloatWTTestCase.class,
			STIntegerWTTestCase.class,
			STLongWTTestCase.class,
			STShortWTTestCase.class,
		};
	}

}
