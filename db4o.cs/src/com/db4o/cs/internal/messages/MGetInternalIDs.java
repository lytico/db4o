/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.cs.internal.objectexchange.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.query.processor.*;

public final class MGetInternalIDs extends MsgD implements MessageWithResponse {
	public final Msg replyFromServer() {
		
		ByteArrayBuffer bytes = getByteLoad();
        final int classMetadataID = bytes.readInt();
        final int prefetchDepth = bytes.readInt();
        final int prefetchCount = bytes.readInt();
        final boolean triggerQueryEvents = bytes.readInt() == 1;
        
		final ByteArrayBuffer payload = marshallIDsFor(classMetadataID,
				prefetchDepth, prefetchCount,
				triggerQueryEvents);
		final MsgD message = Msg.ID_LIST.getWriterForLength(transaction(), payload.length());
		message.payLoad().writeBytes(payload._buffer);
		
		return message;
	}

	private ByteArrayBuffer marshallIDsFor(final int classMetadataID,
			final int prefetchDepth, final int prefetchCount, boolean triggerQueryEvents) {
		synchronized(containerLock()){
			final long[] ids = getIDs(classMetadataID, triggerQueryEvents);
			
			return ObjectExchangeStrategyFactory.forConfig(
					new ObjectExchangeConfiguration(prefetchDepth, prefetchCount)
				).marshall((LocalTransaction)transaction(), IntIterators.forLongs(ids), ids.length);
		}
	}

	private long[] getIDs(final int classMetadataID, boolean triggerQueryEvents) {
		synchronized (containerLock()) {
			final ClassMetadata classMetadata = container().classMetadataForID(classMetadataID);
			if (!triggerQueryEvents) {
				return classMetadata.getIDs(transaction());
			}
			return newQuery(classMetadata).triggeringQueryEvents(new Closure4<long[]>() { public long[] run() {
				return classMetadata.getIDs(transaction());
			}});
		}
			
    }

	private QQuery newQuery(final ClassMetadata classMetadata) {
		final QQuery query = (QQuery)localContainer().query();
		query.constrain(classMetadata);
		return query;
	}
}