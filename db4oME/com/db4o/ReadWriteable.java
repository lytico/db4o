/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * @exclude
 */
public interface ReadWriteable extends Readable{
	public void write(YapReader a_writer);
}
