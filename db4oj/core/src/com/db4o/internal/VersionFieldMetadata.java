/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import com.db4o.ext.*;
import com.db4o.internal.delete.*;
import com.db4o.internal.handlers.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;

/**
 * @exclude
 */
public class VersionFieldMetadata extends VirtualFieldMetadata {

    VersionFieldMetadata() {
        super(Handlers4.LONG_ID, new LongHandler());
        setName(VirtualField.VERSION);
    }
    
    public void addFieldIndex(ObjectIdContextImpl context)  throws FieldIndexException{
        StatefulBuffer buffer = (StatefulBuffer) context.buffer();
        buffer.writeLong(context.transaction().container().generateTimeStampId());
    }
    
    public void delete(DeleteContextImpl context, boolean isUpdate){
        context.seek(context.offset() + linkLength(context));
    }

    void instantiate1(ObjectReferenceContext context) {
        context.objectReference().virtualAttributes().i_version = context.readLong();
    }

    void marshall(Transaction trans, ObjectReference ref, WriteBuffer buffer, boolean isMigrating, boolean isNew) {
        VirtualAttributes attr = ref.virtualAttributes();
        if (! isMigrating) {
            attr.i_version = trans.container().generateTimeStampId();
        }
        if(attr == null){
            buffer.writeLong(0);
        }else{
            buffer.writeLong(attr.i_version);
        }
    }

    public int linkLength(HandlerVersionContext context) {
        return Const4.LONG_LENGTH;
    }
    
    void marshallIgnore(WriteBuffer buffer) {
        buffer.writeLong(0);
    }


}