/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */
package com.db4o.test.reflect;

import com.db4o.reflect.*;

class MockReflectorConfiguration implements ReflectorConfiguration {
	public boolean callConstructor(ReflectClass clazz) {
		return true;
	}

	public boolean testConstructors() {
		return true;
	}
}