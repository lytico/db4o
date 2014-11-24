/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.inside.ix.*;

/**
 * @exclude
 */
public class Null implements Indexable4{
    
    public static final Indexable4 INSTANCE = new Null();

    public Object comparableObject(Transaction trans, Object indexEntry) {
        return null;
    }

    public int compareTo(Object a_obj) {
        if(a_obj == null) {
            return 0;
        }
        return -1;
    }
    
    public Object current(){
        return null;
    }
    
	public boolean equals(Object obj){
		return obj == null;
	}
	
	public boolean isEqual(Object obj) {
		return obj == null;
	}

	public boolean isGreater(Object obj) {
		return false;
	}

	public boolean isSmaller(Object obj) {
		return false;
	}

    public int linkLength() {
        return 0;
    }

	public YapComparable prepareComparison(Object obj) {
		// do nothing
		return this;
	}
	
    public Object readIndexEntry(YapReader a_reader) {
        return null;
    }

    public void writeIndexEntry(YapReader a_writer, Object a_object) {
        // do nothing
    }
}

