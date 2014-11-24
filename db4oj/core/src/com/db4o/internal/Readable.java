/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;



/**
 * @exclude
 */
public interface Readable {
	
	Object read(ByteArrayBuffer buffer);
	
	int marshalledLength();
	
}
