/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.internal.*;


/**
 * @exclude
 */
public class ClassMarshaller2 extends ClassMarshaller {
    
    protected void readIndex(ObjectContainerBase stream, ClassMetadata clazz, ByteArrayBuffer reader) {
        int indexID = reader.readInt();
        if(indexID == 0) {
        	return;
        }
        clazz.index().read(stream, indexID);
    }
    
    protected int indexIDForWriting(int indexID){
        return indexID;
    }

}
