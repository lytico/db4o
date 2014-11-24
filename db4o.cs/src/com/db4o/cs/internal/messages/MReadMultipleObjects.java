/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.cs.internal.objectexchange.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;

public class MReadMultipleObjects extends MsgD implements MessageWithResponse {
	
	public final Msg replyFromServer() {
		int prefetchDepth = readInt();
		int prefetchCount = readInt();
		IntIterator4 ids = new FixedSizeIntIterator4Base(prefetchCount) {
			@Override
			protected int nextInt() {
				return readInt();
			}
		};
		ByteArrayBuffer buffer = marshallObjects(prefetchDepth, prefetchCount, ids);
		
		return Msg.READ_MULTIPLE_OBJECTS.getWriterForBuffer(transaction(), buffer);
	}

	private ByteArrayBuffer marshallObjects(int prefetchDepth, int prefetchCount, IntIterator4 ids) {
		synchronized(containerLock()){
			final ObjectExchangeStrategy strategy = ObjectExchangeStrategyFactory.forConfig(new ObjectExchangeConfiguration(prefetchDepth, prefetchCount));
			return strategy.marshall((LocalTransaction) transaction(), ids, prefetchCount);
		}
	}
}