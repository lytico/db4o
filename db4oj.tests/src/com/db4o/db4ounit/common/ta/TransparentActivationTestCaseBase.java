/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta;

/**
 * @exclude
 */
import com.db4o.config.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class TransparentActivationTestCaseBase
	extends AbstractDb4oTestCase
	implements OptOutTA {

	public TransparentActivationTestCaseBase() {
		super();
	}

	protected void configure(Configuration config) {
		config.add(new TransparentActivationSupport());
		config.generateUUIDs(ConfigScope.GLOBALLY);		
	}

	/**
     * @sharpen.remove
     */
	public void isPrimitiveNull(Object primitive) {
		Assert.isNull(primitive);
	}
}