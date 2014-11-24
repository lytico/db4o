/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

import java.util.*;

/**
 * @exclude
 * @sharpen.ignore
 * @sharpen.rename System.Collections.IEnumerable
 */
@decaf.Ignore(decaf.Platform.JDK11)
public interface IterableBase {
	
	/**
	 * @sharpen.rename GetEnumerator
	 */
	Iterator iterator();
	
}
