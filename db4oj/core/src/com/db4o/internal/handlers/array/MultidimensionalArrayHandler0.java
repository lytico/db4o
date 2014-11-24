/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers.array;

import com.db4o.internal.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;


/**
 * @exclude
 */
public class MultidimensionalArrayHandler0 extends MultidimensionalArrayHandler3 {
    
    protected ArrayVersionHelper createVersionHelper() {
        return new ArrayVersionHelper0();
    }

    public Object read(ReadContext readContext) {
        InternalReadContext context = (InternalReadContext) readContext;
        
        ByteArrayBuffer buffer = (ByteArrayBuffer) context.readIndirectedBuffer();
        if (buffer == null) {
            return null;
        }
        
        // With the following line we ask the context to work with 
        // a different buffer. Should this logic ever be needed by
        // a user handler, it should be implemented by using a Queue
        // in the UnmarshallingContext.
        
        // The buffer has to be set back from the outside!  See below
        ReadBuffer contextBuffer = context.buffer(buffer);
        
        Object array = super.read(context);
        
        // The context buffer has to be set back.
        context.buffer(contextBuffer);
        
        return array;
    }

    public void defragment(DefragmentContext context) {
        ArrayHandler0.defragment(context, this);
    }
    

}
