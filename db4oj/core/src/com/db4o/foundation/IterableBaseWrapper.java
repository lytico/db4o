/* Copyright (C) 2008  Versant Inc.   http://www.db4o.com */
package com.db4o.foundation;

/**
 * @exclude
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public interface IterableBaseWrapper extends IterableBase {
	
	Object delegate();
	
}
