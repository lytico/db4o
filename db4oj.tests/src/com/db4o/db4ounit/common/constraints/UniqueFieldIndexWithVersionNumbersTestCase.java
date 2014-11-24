package com.db4o.db4ounit.common.constraints;

import com.db4o.config.*;

public class UniqueFieldIndexWithVersionNumbersTestCase extends
		UniqueFieldValueConstraintTestCase {

	protected void configure(Configuration config) throws Exception {
		super.configure(config);
		config.generateCommitTimestamps(true);
	}
}
