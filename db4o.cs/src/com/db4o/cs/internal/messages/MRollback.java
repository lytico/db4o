/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;


public final class MRollback extends Msg implements ServerSideMessage {
	
	public final void processAtServer() {
		synchronized (containerLock()) {
			transaction().rollback();
		}
	}
	
}