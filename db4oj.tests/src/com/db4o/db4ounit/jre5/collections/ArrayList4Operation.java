/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre5.collections;

import com.db4o.collections.*;

/**
 * @exclude
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public interface ArrayList4Operation <T> {
	public void operate(ArrayList4<T> list);
}
