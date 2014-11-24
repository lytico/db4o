/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;


/**
 * @exclude
 */
public class MClose extends Msg implements ServerSideMessage, ClientSideMessage {
	
	public void processAtServer() {
		synchronized (containerLock()) {
			if (container().isClosed()) {
				return;
			}
			transaction().commit();
			logMsg(35, serverMessageDispatcher().name());
			serverMessageDispatcher().close();
		}
	}
	
	public boolean processAtClient() {
		clientMessageDispatcher().close();
        return true; 
	}
}
