/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;

/**
 * @exclude
 */
public class MRaiseCommitTimestamp extends MsgD implements ServerSideMessage {

	public void processAtServer() {
		container().raiseCommitTimestamp(readLong());
	}
}
