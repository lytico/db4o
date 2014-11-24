/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.internal.query.result.*;


public class MObjectSetSize extends MObjectSet implements MessageWithResponse {
	
	public Msg replyFromServer() {
		synchronized(containerLock()) {
			AbstractQueryResult queryResult = queryResult(readInt());
			return Msg.OBJECTSET_SIZE.getWriterForInt(transaction(), queryResult.size());
		}
	}
	
}
