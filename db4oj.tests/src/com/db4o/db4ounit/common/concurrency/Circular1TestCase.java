/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.concurrency;

import com.db4o.ext.*;

import db4ounit.extensions.*;

/**
 * 
 */
public class Circular1TestCase extends Db4oClientServerTestCase {
	public static void main(String[] args) {
		new Circular1TestCase().runConcurrency();
	}

	protected void store() {
		store(new C1C());
	}

	public void conc(ExtObjectContainer oc) {
		assertOccurrences(oc, C1C.class, 1);
	}

	public static class C1P {
		C1C c;
	}

	public static class C1C extends C1P {
	}
}
