/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.internal.*;


/**
 * @exclude
 */
public abstract class AbstractFieldMarshaller implements FieldMarshaller {
    
    protected abstract RawFieldSpec readSpec(AspectType aspectType, ObjectContainerBase stream,ByteArrayBuffer reader);
    
    public RawFieldSpec readSpec(ObjectContainerBase stream,ByteArrayBuffer reader){
        return readSpec(AspectType.FIELD, stream, reader);
    }

}
