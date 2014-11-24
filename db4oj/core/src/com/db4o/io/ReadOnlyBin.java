/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */
package com.db4o.io;

import com.db4o.ext.*;

/**
 * @exclude
 */
public class ReadOnlyBin extends BinDecorator {

	public ReadOnlyBin(Bin storage) {
	    super(storage);
    }
	
	@Override
	public void write(long position, byte[] bytes, int bytesToWrite) {
	    throw new Db4oIOException();
	}

}
