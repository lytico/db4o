/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.marshall;


/**
 * @exclude
 */
public interface BufferContext extends ReadBuffer, Context {

    public ReadBuffer buffer();

}
