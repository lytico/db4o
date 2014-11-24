/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.handlers;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;


public class MockReadContext extends MockMarshallingContext implements ReadContext{

    public MockReadContext(ObjectContainer objectContainer) {
        super(objectContainer);
    }

    public MockReadContext(MockWriteContext writeContext) {
        this(writeContext.objectContainer());
        writeContext._header.copyTo(_header, 0, 0, writeContext._header.length());
        writeContext._payLoad.copyTo(_payLoad, 0, 0, writeContext._payLoad.length());
    }
    
    public Object readObject(TypeHandler4 handler) {
        return Handlers4.readValueType(this, handler);
    }

    public BitMap4 readBitMap(int bitCount) {
        BitMap4 map = new BitMap4(_current._buffer, _current._offset, bitCount);
        _current.seek(_current.offset() + map.marshalledLength());
        return map;
    }

}
