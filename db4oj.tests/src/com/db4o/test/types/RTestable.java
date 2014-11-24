/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.types;

import com.db4o.*;

public interface RTestable {	Object newInstance();
	Object set(Object obj, int ver);
	void compare(ObjectContainer con, Object obj, int ver);	boolean jdk2();	boolean ver3();
}