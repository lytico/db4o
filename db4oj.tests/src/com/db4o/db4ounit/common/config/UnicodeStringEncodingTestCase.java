/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.config;

import com.db4o.internal.encoding.*;

/**
 * @exclude
 */
public class UnicodeStringEncodingTestCase extends StringEncodingTestCaseBase {

	protected Class stringIoClass() {
		return UnicodeStringIO.class;
	}

}
