/* Copyright (C) 2004 - 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.marshall.*;

/**
 * @exclude
 */
public interface Indexable4<T> extends Comparable4<T>, LinkLengthAware{
    
    T readIndexEntry(Context context, ByteArrayBuffer reader);
    
    void writeIndexEntry(Context context, ByteArrayBuffer writer, T obj);
    
	void defragIndexEntry(DefragmentContextImpl context);
	
}

