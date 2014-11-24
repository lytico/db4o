package com.db4o.cs.internal.messages;

/**
 * @exclude
 */
public class MCloseSocket extends Msg implements ServerSideMessage {

	public void processAtServer() {
		synchronized (containerLock()) {
			if (container().isClosed()) {
				return;
			}
			transaction().commit();
			logMsg(35, serverMessageDispatcher().name());
			serverMessageDispatcher().closeConnection();
		}
	}
}
