/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.internal.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public class SlotFormat0 extends SlotFormat {

    protected int handlerVersion() {
        return 0;
    }
    
    public boolean isIndirectedWithinSlot(TypeHandler4 handler){
        return false;
    }
    
    public void writeObjectClassID(ByteArrayBuffer buffer, int id) {
        buffer.writeInt(id);
    }
    
    public void skipMarshallerInfo(ByteArrayBuffer reader) {
    }
    
    public ObjectHeaderAttributes readHeaderAttributes(ByteArrayBuffer reader) {
        return null;
    }




}
