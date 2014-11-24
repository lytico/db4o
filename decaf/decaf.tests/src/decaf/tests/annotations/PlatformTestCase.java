/* Copyright (C) 2011  Versant Inc.   http://www.db4o.com */
package decaf.tests.annotations;

import static junit.framework.Assert.*;

import org.junit.*;

import decaf.*;

public class PlatformTestCase {
	
	@Test
	public void testTransitiveness() {
		assertTrue(Platform.ANDROID.compatibleWith(Platform.JDK12));
		assertTrue(Platform.JMX.compatibleWith(Platform.JMX));
		assertTrue(Platform.JDK15.compatibleWith(Platform.JMX));
	}
	
	@Test
	public void testNegation() {
		assertFalse(Platform.ANDROID.compatibleWith(Platform.JMX));
		assertFalse(Platform.JDK12.compatibleWith(Platform.JMX));
		assertFalse(Platform.JMX.compatibleWith(Platform.JDK15));
	}

}
