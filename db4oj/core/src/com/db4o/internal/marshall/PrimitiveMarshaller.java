/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import java.util.*;

import com.db4o.internal.*;


public abstract class PrimitiveMarshaller {
    
    public MarshallerFamily _family;
    
    public abstract boolean useNormalClassRead();
    
    public abstract Date readDate(ByteArrayBuffer bytes);
    
    public abstract Object readShort(ByteArrayBuffer buffer);
    
    public abstract Object readInteger(ByteArrayBuffer buffer);
    
    public abstract Object readFloat(ByteArrayBuffer buffer);
    
    public abstract Object readDouble(ByteArrayBuffer buffer);
    
    public abstract Object readLong(ByteArrayBuffer buffer);
    
}
