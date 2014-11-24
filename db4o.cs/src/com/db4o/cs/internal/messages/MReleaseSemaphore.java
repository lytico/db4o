/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

public final class MReleaseSemaphore extends MsgD implements ServerSideMessage {
	
	public final void processAtServer() {
		localContainer().releaseSemaphore(transaction(),readString());
	}
	
}