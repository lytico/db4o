/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */
package com.db4o.cs.internal;

public interface BroadcastFilter {
	public boolean accept(ServerMessageDispatcher dispatcher);
}
