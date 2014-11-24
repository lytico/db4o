/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre5.concurrency.query;

import java.util.*;

/**
 * @sharpen.ignore
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class IteratorPlatform {

	static Object next(final Iterator result) {
		return result.next();
	}

}
