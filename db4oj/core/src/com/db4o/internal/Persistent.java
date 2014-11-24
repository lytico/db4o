/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

/**
 * @exclude
 */
public interface Persistent {

	byte getIdentifier();

	int ownLength();

	void readThis(Transaction trans, ByteArrayBuffer reader);

	void writeThis(Transaction trans, ByteArrayBuffer writer);

}