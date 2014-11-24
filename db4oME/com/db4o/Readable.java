/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;


/**
 * @exclude
 */
public interface Readable {
	Object read(YapReader a_reader);
	int byteCount();
}
