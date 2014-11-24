package com.db4o.db4ounit.optional.handlers;

import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;
import db4ounit.fixtures.*;

/**
 * @sharpen.remove
 */
public class BigNumbersTestSuite extends FixtureTestSuiteDescription implements Db4oTestCase {{
	
	fixtureProviders(
			new Db4oFixtureProvider(),
			new SubjectFixtureProvider(true, false)); // indexing configuration
	
	testUnits(
		BigIntegerTypeHandlerTestCase.class,
		BigDecimalTypeHandlerTestCase.class);
	
}}