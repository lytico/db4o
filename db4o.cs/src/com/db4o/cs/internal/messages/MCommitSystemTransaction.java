/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;



/**
 * @exclude
 */
public class MCommitSystemTransaction extends Msg implements ServerSideMessage {
	
	public final void processAtServer() {
		synchronized (containerLock()) {
			transaction().systemTransaction().commit();
		}
	}

}
