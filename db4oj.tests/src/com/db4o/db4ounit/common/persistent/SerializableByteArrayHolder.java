/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.persistent;

import java.io.*;

public class SerializableByteArrayHolder implements Serializable, IByteArrayHolder {

	private static final long serialVersionUID = 1L;

	public byte[] _bytes;

	public SerializableByteArrayHolder(byte[] bytes) {
		this._bytes = bytes;
	}

	public byte[] getBytes() {
		return _bytes;
	}
}

