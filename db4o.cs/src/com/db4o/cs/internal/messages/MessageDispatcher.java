/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;



/**
 * @exclude
 */
public interface MessageDispatcher {

	public boolean isMessageDispatcherAlive();
	
	public boolean write(Msg msg);
	
	public boolean close();
}
