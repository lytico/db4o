/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

public final class MSetSemaphore extends MsgD implements MessageWithResponse {
	
	public final Msg replyFromServer() {
		int timeout = readInt();
		String name = readString();
		boolean res = localContainer().setSemaphore(transaction(), name, timeout);
		return (res ? (Msg)Msg.SUCCESS : Msg.FAILED);
	}

}