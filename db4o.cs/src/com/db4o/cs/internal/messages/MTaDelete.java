/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.internal.*;

public class MTaDelete extends MsgD implements ServerSideMessage {
	
	public final void processAtServer() {
	    int id = _payLoad.readInt();
	    int cascade = _payLoad.readInt();
	    Transaction trans = transaction();
	    synchronized (containerLock()) {
	        trans.delete(null, id, cascade);
	    }
	}
}
