package com.db4o.db4ounit.common.assorted;

import db4ounit.extensions.*;

public class UnavailableClassTestCaseBase extends AbstractDb4oTestCase {

	public UnavailableClassTestCaseBase() {
		super();
	}

	protected void reopenHidingClasses(final Class<?>... classes) throws Exception {
		fixture().close();
    	fixture().config().reflectWith(new ExcludingReflector(classes));
    	open();
    }

	private void open() throws Exception {
        fixture().open(this);
    }

	private void closeAndResetConfig() throws Exception {
    	fixture().close();
    	fixture().resetConfig();
    }
}