/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

/**
 * @exclude
 */
import com.db4o.internal.*;
import com.db4o.marshall.*;

public class ObjectReferenceContext extends ObjectHeaderContext implements ObjectIdContext{

    protected final ObjectReference _reference;

    public ObjectReferenceContext(Transaction transaction, ReadBuffer buffer,
        ObjectHeader objectHeader, ObjectReference reference) {
        super(transaction, buffer, objectHeader);
        _reference = reference;
    }

    public int objectId() {
        return _reference.getID();
    }

    public ClassMetadata classMetadata() {
        final ClassMetadata classMetadata = _reference.classMetadata();
        if (classMetadata == null) {
        	throw new IllegalStateException();
        }
		return classMetadata;
    }

    public ObjectReference objectReference() {
        return _reference;
    }
    
    protected ByteArrayBuffer byteArrayBuffer() {
        return (ByteArrayBuffer)buffer();
    }

}