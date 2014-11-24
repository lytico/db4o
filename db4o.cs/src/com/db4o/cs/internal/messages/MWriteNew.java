/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.internal.*;
import com.db4o.internal.slots.*;

public final class MWriteNew extends MsgObject implements ServerSideMessage {
	
	public final void processAtServer() {
        int classMetadataId = _payLoad.readInt();
        unmarshall(_payLoad._offset);
        synchronized (containerLock()) {
            ClassMetadata classMetadata = classMetadataId == 0
            					? null
            					: localContainer().classMetadataForID(classMetadataId);
            
            int id = _payLoad.getID();
            
            transaction().idSystem().prefetchedIDConsumed(id);            
            
            Slot slot = localContainer().allocateSlotForNewUserObject(transaction(), id, _payLoad.length());
            
            _payLoad.address(slot.address());
            
            if(classMetadata != null){
                classMetadata.addFieldIndices(_payLoad);
            }
            localContainer().writeNew(transaction(), _payLoad.pointer(), classMetadata, _payLoad);
            
        }
    }
}