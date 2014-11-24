/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.marshall.*;


/**
 * @exclude
 */
public interface ReadWriteBuffer extends ReadBuffer, WriteBuffer{

	void incrementOffset(int numBytes);
	void incrementIntSize();

    int length();
    
	void readBegin(byte identifier);
	void readEnd();

}