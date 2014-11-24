/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers.versions;

import java.io.*;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.internal.mapping.*;
import com.db4o.internal.marshall.*;
import com.db4o.internal.slots.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public class OpenTypeHandler0 extends OpenTypeHandler2 {

    public OpenTypeHandler0(ObjectContainerBase container) {
        super(container);
    }
    
    @Override
    public Object read(ReadContext context) {
        return context.readObject();
    }
    
    public TypeHandler4 readCandidateHandler(QueryingReadContext context) {
        int id = 0;

        int offset = context.offset();
        try {
            id = context.readInt();
        } catch (Exception e) {
        }
        context.seek(offset);

        if (id != 0) {
            StatefulBuffer reader =
                context.container().readStatefulBufferById(context.transaction(), id);
            if (reader != null) {
                ObjectHeader oh = new ObjectHeader(context.container(), reader);
                try {
                    if (oh.classMetadata() != null) {
                        context.buffer(reader);
                        return oh.classMetadata().seekCandidateHandler(context);
                    }
                } catch (Exception e) {
                    
                    if(Debug4.atHome){
                        e.printStackTrace();
                    }
                    
                    // TODO: Check Exception Types
                    // Errors typically occur, if classes don't match
                }
            }
        }
        return null;
    }
    
    public ObjectID readObjectID(InternalReadContext context){
        int id = context.readInt();
        return id == 0 ? ObjectID.IS_NULL : new ObjectID(id);
    }
    
    public void defragment(DefragmentContext context) {
        int sourceId = context.sourceBuffer().readInt();
        if(sourceId == 0) {
            context.targetBuffer().writeInt(0);
            return;
        }
        int targetId = 0;
        try {
        	targetId = context.mappedID(sourceId);
        }
        catch(MappingNotFoundException exc) {
        	targetId = copyDependentSlot(context, sourceId);
        }
        context.targetBuffer().writeInt(targetId);
    }

	private int copyDependentSlot(DefragmentContext context, int sourceId) {
		try {
			ByteArrayBuffer sourceBuffer = context.sourceBufferById(sourceId);
			Slot targetPayloadSlot = context.allocateTargetSlot(sourceBuffer.length());
			int targetId = context.services().targetNewId();
			context.services().mapIDs(sourceId, targetId, false);
			context.services().mapping().mapId(targetId, targetPayloadSlot);
			DefragmentContextImpl payloadContext = new DefragmentContextImpl(sourceBuffer, (DefragmentContextImpl) context);
			int clazzId = payloadContext.copyIDReturnOriginalID();
			TypeHandler4 payloadHandler = payloadContext.typeHandlerForId(clazzId);
			TypeHandler4 versionedPayloadHandler = HandlerRegistry.correctHandlerVersion(payloadContext, payloadHandler);
			versionedPayloadHandler.defragment(payloadContext);
			payloadContext.writeToTarget(targetPayloadSlot.address());
			return targetId;
		}
		catch (IOException ioexc) {
			throw new Db4oIOException(ioexc);
		}
	}

}
