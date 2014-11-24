/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.internal.*;
import com.db4o.internal.encoding.*;


/**
 * @exclude
 */
public class FieldMarshaller2 extends FieldMarshaller1 {
    
    private static final int ASPECT_TYPE_TAG_LENGTH = 1;
    
    public int marshalledLength(ObjectContainerBase stream, ClassAspect aspect) {
        return super.marshalledLength(stream, aspect) + ASPECT_TYPE_TAG_LENGTH;
    }
    
    protected RawFieldSpec readSpec(AspectType aspectType, ObjectContainerBase stream, ByteArrayBuffer reader) {
        return super.readSpec(AspectType.forByte(reader.readByte()), stream, reader);
    }
    
    public void write(Transaction trans, ClassMetadata clazz, ClassAspect aspect, ByteArrayBuffer writer) {
        writer.writeByte(aspect.aspectType()._id);
        super.write(trans, clazz, aspect, writer);
    }
    
    public void defrag(ClassMetadata classMetadata, ClassAspect aspect, LatinStringIO sio,
        final DefragmentContextImpl context){
        context.readByte();
        super.defrag(classMetadata, aspect, sio, context);
    }

}
