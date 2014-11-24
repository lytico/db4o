/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.internal.*;
import com.db4o.internal.ids.TransactionalIdSystem;

public final class MPrefetchIDs extends MsgD implements MessageWithResponse {

	public final Msg replyFromServer() {
		int prefetchIDCount = readInt();
		MsgD reply = Msg.ID_LIST.getWriterForLength(transaction(), Const4.INT_LENGTH * prefetchIDCount);

		synchronized (containerLock()) {
			TransactionalIdSystem idSystem = transaction().idSystem();
			for (int i = 0; i < prefetchIDCount; i++) {
				reply.writeInt(idSystem.prefetchID());
			}
		}
		return reply;
	}
}