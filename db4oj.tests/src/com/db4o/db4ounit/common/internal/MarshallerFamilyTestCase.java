/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.internal;

import com.db4o.ext.*;
import com.db4o.internal.marshall.*;

import db4ounit.*;

public class MarshallerFamilyTestCase implements TestCase{
	
	public void testThrowingOnNewerVersion(){
		Assert.expect(IncompatibleFileFormatException.class, new CodeBlock() {
			public void run() throws Throwable {
				MarshallerFamily.version(Integer.MAX_VALUE);
			}
		});
		
	}

}
