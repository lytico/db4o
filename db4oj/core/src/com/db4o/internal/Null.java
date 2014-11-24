/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;
import com.db4o.marshall.*;


/**
 * @exclude
 */
public class Null implements Indexable4, PreparedComparison{
    
    public static final Null INSTANCE = new Null();
    
    private Null() {
    }

    public int compareTo(Object a_obj) {
        if(a_obj == null) {
            return 0;
        }
        return -1;
    }
    
    public int linkLength() {
        return 0;
    }

    public Object readIndexEntry(Context context, ByteArrayBuffer a_reader) {
        return null;
    }

    public void writeIndexEntry(Context context, ByteArrayBuffer a_writer, Object a_object) {
        // do nothing
    }

	public void defragIndexEntry(DefragmentContextImpl context) {
        // do nothing
	}

	public PreparedComparison prepareComparison(Context context, Object obj_) {
		return new PreparedComparison() {
			public int compareTo(Object obj) {
				if(obj == null){
					return 0;
				}
				if(obj instanceof Null){
					return 0;
				}
				return -1;
			}
		};
	}
}

