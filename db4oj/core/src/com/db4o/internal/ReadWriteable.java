/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;


/**
 * @exclude
 */
public interface ReadWriteable extends Readable{
	
	public void write(ByteArrayBuffer buffer);
	
}
