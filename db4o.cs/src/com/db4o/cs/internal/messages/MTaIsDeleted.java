/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;


public final class MTaIsDeleted extends MsgD implements MessageWithResponse {
	
	public final Msg replyFromServer() {
		synchronized (containerLock()) {
			boolean isDeleted = container().isDeleted(transaction(), readInt());
			int ret = isDeleted ? 1 : 0;
			return Msg.TA_IS_DELETED.getWriterForInt(transaction(), ret);
		}
	}
}