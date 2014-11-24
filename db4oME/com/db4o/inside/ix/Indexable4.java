/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.ix;

import com.db4o.*;

/**
 * @exclude
 */
public interface Indexable4 extends YapComparable{
    
    Object comparableObject(Transaction trans, Object indexEntry);

    int linkLength();
    
    Object readIndexEntry(YapReader a_reader);
    
    void writeIndexEntry(YapReader a_writer, Object a_object);

}
