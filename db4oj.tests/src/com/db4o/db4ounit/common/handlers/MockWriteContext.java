/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.handlers;

import com.db4o.*;
import com.db4o.internal.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;


public class MockWriteContext extends MockMarshallingContext implements WriteContext{

    public MockWriteContext(ObjectContainer objectContainer) {
        super(objectContainer);
    }
    
    public void writeObject(TypeHandler4 handler, Object obj) {
        Handlers4.write(handler, this, obj);
    }

    public void writeAny(Object obj) {
        ClassMetadata classMetadata = container().classMetadataForObject(obj);
        writeInt(classMetadata.getID());
        Handlers4.write(classMetadata.typeHandler(), this, obj);
    }

    public ReservedBuffer reserve(int length) {
        ReservedBuffer reservedBuffer = new ReservedBuffer() {
            private final int reservedOffset = offset();
            public void writeBytes(byte[] bytes) {
                int currentOffset = offset();
                seek(reservedOffset);
                MockWriteContext.this.writeBytes(bytes);
                seek(currentOffset);
            }
        };
        seek(offset() + length );
        return reservedBuffer;
    }
    
}
