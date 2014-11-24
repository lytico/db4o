/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.query;

import com.db4o.*;
import com.db4o.query.*;

/**
 * @exclude
 */
public interface QueryResult {

    public Object get(int index);

    public long[] getIDs();

    public boolean hasNext();

    public Object next();

    public void reset();
    
    public int size();
    
    public Object streamLock();
    
    public ObjectContainer objectContainer();
    
    public int indexOf(int id);

	public void sort(QueryComparator cmp);
}