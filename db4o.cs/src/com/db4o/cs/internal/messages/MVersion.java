/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;

/**
 * @exclude
 */
public class MVersion extends Msg implements MessageWithResponse {

	public Msg replyFromServer() {
		long ver = 0;
		synchronized (containerLock()) {
			ver = container().currentVersion();
		}
		return Msg.ID_LIST.getWriterForLong(transaction(), ver);
	}
}
