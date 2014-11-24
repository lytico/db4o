/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.internal.query.result.*;


/**
 * @exclude
 */
public class MObjectSetGetId extends MObjectSet implements MessageWithResponse {
	
	public Msg replyFromServer() {
		AbstractQueryResult queryResult = queryResult(readInt());		
		int id = 0;
		synchronized (containerLock()) {
			id = queryResult.getId(readInt());
		}
		return Msg.OBJECTSET_GET_ID.getWriterForInt(transaction(), id);
	}

}
