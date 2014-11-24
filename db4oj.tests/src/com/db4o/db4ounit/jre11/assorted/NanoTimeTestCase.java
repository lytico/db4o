/* Copyright (C) 2004 - 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre11.assorted;

import com.db4o.foundation.*;
import com.db4o.internal.*;

import db4ounit.*;


public class NanoTimeTestCase implements TestCase {

	public void testCorrectExceptionThrownOnJdkLowerThan5() {
		if (jdkVer() >= 5) {
			return;
		}
		Assert.expect(NotImplementedException.class, new CodeBlock() {
			public void run() {
				Platform4.jdk().nanoTime();
			}
		});
	}
	
	public void testNanoTimeAvailableOnJdk5Plus() {
		if (jdkVer() < 5) {
			return;
		}
		try {
			Platform4.jdk().nanoTime();
		} catch (Exception e) {
			Assert.fail("nanoTime should be available on JDK 5 and higher.", e);
		}
	}
	
	private int jdkVer() {
		return Platform4.jdk().ver();
	}
	
}
