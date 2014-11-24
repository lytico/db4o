/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;


public final class MVersionForId extends MsgD implements MessageWithResponse {

	public final Msg replyFromServer() {
		synchronized (containerLock()) {
			long version = systemTransaction().versionForId(readInt());
			return Msg.VERSION_FOR_ID.getWriterForLong(transaction(), version);
		}
	}
}