/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.config;

import com.db4o.config.*;
import com.db4o.config.encoding.*;
import com.db4o.internal.encoding.*;

import db4ounit.*;

public class UTF8StringEncodingTestCase extends StringEncodingTestCaseBase {
	
	protected void configure(Configuration config) throws Exception {
		config.stringEncoding(StringEncodings.utf8());
	}

	protected Class stringIoClass() {
		return DelegatingStringIO.class;
	}
	
	public static void main(String[] arguments) {
		new UTF8StringEncodingTestCase().runEmbedded();
	}
	
	public void testEncodeDecode() {
		String original = "ABCZabcz?$@#.,;:";
		UTF8StringEncoding encoder = new UTF8StringEncoding();
		byte[] bytes = encoder.encode(original);
		String decoded = encoder.decode(bytes, 0, bytes.length);
		Assert.areEqual(original, decoded);
	}

}
