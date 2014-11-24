/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.cs.internal.*;
import com.db4o.internal.query.result.*;


/**
 * @exclude
 */
public abstract class MObjectSet extends MsgD {
	
	protected AbstractQueryResult queryResult(int queryResultID){
		return stub(queryResultID).queryResult();
	}

	protected LazyClientObjectSetStub stub(int queryResultID) {
		ServerMessageDispatcher serverThread = serverMessageDispatcher();
		return serverThread.queryResultForID(queryResultID);
	}

}
