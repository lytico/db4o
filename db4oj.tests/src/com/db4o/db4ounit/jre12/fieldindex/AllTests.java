/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre12.fieldindex;

import db4ounit.extensions.*;

/**
 * @exclude
 */
/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class AllTests extends Db4oTestSuite {

	protected Class[] testCases() {
		return new Class[] { CollectionFieldIndexTestCase.class };
	}

}
