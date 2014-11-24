/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit.extensions;

import com.db4o.config.*;
import com.db4o.ta.*;

/**
 * Configure the test case to run with TransparentActivationSupport
 * enabled unless the test case implements OptOutTA.
 */
public class TAFixtureConfiguration implements FixtureConfiguration {

	public void configure(Db4oTestCase testCase, Configuration config) {
		if (testCase instanceof OptOutTA) {
			return;
		}
		config.add(new TransparentActivationSupport());
	}

	public String getLabel() {
		return "TA";
	}
}
