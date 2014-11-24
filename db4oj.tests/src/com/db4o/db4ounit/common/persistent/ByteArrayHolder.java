/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.persistent;


public class ByteArrayHolder implements IByteArrayHolder {

	public byte[] _bytes;

	public ByteArrayHolder(byte[] bytes) {
		this._bytes = bytes;
	}

	public byte[] getBytes() {
		return _bytes;
	}
}
