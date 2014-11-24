/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public abstract class SlotFormat {
    
    private static final Hashtable4 _versions = new Hashtable4();
    
    private static final SlotFormat CURRENT_SLOT_FORMAT = new SlotFormatCurrent() ;
    
    static{
        new SlotFormat0();
        new SlotFormat2();
    }
    
    protected SlotFormat(){
        _versions.put(handlerVersion(), this);
    }
    
    public static final SlotFormat forHandlerVersion(int handlerVersion){
        if(handlerVersion == HandlerRegistry.HANDLER_VERSION){
            return CURRENT_SLOT_FORMAT; 
        }
        if(handlerVersion < 0  || handlerVersion > CURRENT_SLOT_FORMAT.handlerVersion()){
            throw new IllegalArgumentException();
        }
        SlotFormat slotFormat = (SlotFormat) _versions.get(handlerVersion);
        if(slotFormat != null){
            return slotFormat;
        }
        return forHandlerVersion(handlerVersion + 1);
    }
    
    public boolean equals(Object obj) {
        if(! (obj instanceof SlotFormat)){
            return false;
        }
        return handlerVersion() == ((SlotFormat)obj).handlerVersion();
    }
    
    public int hashCode() {
        return handlerVersion();
    }
    
    protected abstract int handlerVersion();

    public abstract boolean isIndirectedWithinSlot(TypeHandler4 handler);
    
    public static SlotFormat current(){
        return CURRENT_SLOT_FORMAT;
    }
    
    public Object doWithSlotIndirection(ReadBuffer buffer, TypeHandler4 typeHandler, Closure4 closure){
        if(! isIndirectedWithinSlot(typeHandler)){
            return closure.run();
        }
        return doWithSlotIndirection(buffer, closure);
    }
    
    public Object doWithSlotIndirection(ReadBuffer buffer, Closure4 closure){
        int payLoadOffset = buffer.readInt();
        buffer.readInt(); // length, not used
        int savedOffset = buffer.offset();
        Object res = null;
        if(payLoadOffset != 0){
            buffer.seek(payLoadOffset);
            res = closure.run();
        }
        buffer.seek(savedOffset);
        return res;
    }
    
    public void writeObjectClassID(ByteArrayBuffer buffer, int id) {
        buffer.writeInt(-id);
    }
    
    public void skipMarshallerInfo(ByteArrayBuffer reader) {
        reader.incrementOffset(1);
    }

    public ObjectHeaderAttributes readHeaderAttributes(ByteArrayBuffer reader) {
        return new ObjectHeaderAttributes(reader);
    }
    
}
