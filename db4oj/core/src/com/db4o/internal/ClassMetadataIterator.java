/* Copyright (C) 2004 - 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;


/**
 * @exclude
 * 
 * TODO: remove this class or make it private to ClassMetadataRepository
 */
public class ClassMetadataIterator extends MappingIterator {
    
    private final ClassMetadataRepository i_collection;
    
    ClassMetadataIterator(ClassMetadataRepository a_collection, Iterator4 iterator){
        super(iterator);
        i_collection = a_collection;
    }
    
    public ClassMetadata currentClass() {
        return (ClassMetadata)current();
    }
    
	protected Object map(Object current) {
		return i_collection.readClassMetadata((ClassMetadata)current, null);
	}
}
