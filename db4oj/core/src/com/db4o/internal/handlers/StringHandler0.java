/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers;

import java.io.*;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.internal.delete.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;


/**
 * @exclude
 */
public class StringHandler0 extends StringHandler {

    public Object read(ReadContext context) {
        ByteArrayBuffer buffer = (ByteArrayBuffer) ((InternalReadContext)context).readIndirectedBuffer();
        if (buffer == null) {
            return null;
        }
        return readString(context, buffer);
    }
    
    public void delete(DeleteContext context){
    	context.defragmentRecommended();
    }
    
    public void defragment(DefragmentContext context) {
    	int sourceAddress = context.sourceBuffer().readInt();
    	int length = context.sourceBuffer().readInt();
    	if(sourceAddress == 0 && length == 0) {
        	context.targetBuffer().writeInt(0);
        	context.targetBuffer().writeInt(0);
        	return;
    	}

    	int targetAddress = 0;
    	try {
			targetAddress = context.copySlotToNewMapped(sourceAddress, length);
		} 
    	catch (IOException exc) {
    		throw new Db4oIOException(exc);
		}
    	context.targetBuffer().writeInt(targetAddress);
    	context.targetBuffer().writeInt(length);
    }
    
    public Object readIndexEntryFromObjectSlot(MarshallerFamily mf, StatefulBuffer buffer) throws CorruptionException, Db4oIOException {
        return buffer.container().readWriterByAddress(buffer.transaction(), buffer.readInt(), buffer.readInt());
    }
    
    public Object readIndexEntry(ObjectIdContext context) throws CorruptionException, Db4oIOException{
        return context.transaction().container().readWriterByAddress(context.transaction(), context.readInt(), context.readInt());
    }

}
