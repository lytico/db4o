/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.cs.internal.*;


public final class MUseTransaction extends MsgD implements ServerSideMessage {

	public void processAtServer() {
		ServerMessageDispatcher serverThread = serverMessageDispatcher();
		serverThread.useTransaction(this);
	}
}