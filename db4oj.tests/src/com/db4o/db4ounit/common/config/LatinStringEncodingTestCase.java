/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.config;

import com.db4o.config.*;
import com.db4o.config.encoding.*;
import com.db4o.internal.encoding.*;

/**
 * @exclude
 */
public class LatinStringEncodingTestCase extends StringEncodingTestCaseBase {

	protected Class stringIoClass() {
		return LatinStringIO.class;
	}
	
	protected void configure(Configuration config) throws Exception {
		config.stringEncoding(StringEncodings.latin());
	}


}
