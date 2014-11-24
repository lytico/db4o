/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.cs.internal.objectexchange.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;


/**
 * @exclude
 */
public class MObjectSetFetch extends MObjectSet implements MessageWithResponse {
	
	public Msg replyFromServer() {
		int queryResultID = readInt();
		int fetchSize = readInt();
		int fetchDepth = readInt();
		MsgD message = null;
		synchronized(containerLock()) {
			IntIterator4 idIterator = stub(queryResultID).idIterator();
			ByteArrayBuffer payload = ObjectExchangeStrategyFactory.forConfig(new ObjectExchangeConfiguration(fetchDepth, fetchSize)).marshall((LocalTransaction) transaction(), idIterator, fetchSize);
			message = ID_LIST.getWriterForBuffer(transaction(), payload);
		}
		return message;
	}

}
