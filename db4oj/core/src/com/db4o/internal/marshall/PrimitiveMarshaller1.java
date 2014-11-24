/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import java.util.*;

import com.db4o.internal.*;


public class PrimitiveMarshaller1 extends PrimitiveMarshaller {
    
    public boolean useNormalClassRead(){
        return false;
    }
    
    public Date readDate(ByteArrayBuffer bytes){
		return new Date(bytes.readLong());
	}
    
    public Object readInteger(ByteArrayBuffer bytes) {
    	return new Integer(bytes.readInt());
    }
    
    public Object readFloat(ByteArrayBuffer bytes) {
    	return PrimitiveMarshaller0.unmarshallFloat(bytes);
    }

	public Object readDouble(ByteArrayBuffer buffer) {
		return PrimitiveMarshaller0.unmarshalDouble(buffer);
	}

	public Object readLong(ByteArrayBuffer buffer) {
		return new Long(buffer.readLong());
	}

	public Object readShort(ByteArrayBuffer buffer) {
		return new Short(PrimitiveMarshaller0.unmarshallShort(buffer));
	}

}
