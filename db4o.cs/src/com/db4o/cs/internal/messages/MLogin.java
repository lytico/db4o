/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.*;
import com.db4o.cs.internal.*;

/**
 * @exclude
 */
public class MLogin extends MsgD implements MessageWithResponse {

	public Msg replyFromServer() {
		synchronized (containerLock()) {
		    String userName = readString();
		    String password = readString();
		    ObjectServerImpl server = serverMessageDispatcher().server();
    		User found = server.getUser(userName);
    		if (found != null) {
    			if (found.password.equals(password)) {
    				serverMessageDispatcher().setDispatcherName(userName);
    				logMsg(32, userName);
    				int blockSize = container().blockSize();
    				int encrypt = container()._handlers.i_encrypt ? 1 : 0;
    				serverMessageDispatcher().login();
    				return Msg.LOGIN_OK.getWriterForInts(transaction(), new int[] { blockSize, encrypt, serverMessageDispatcher().dispatcherID() });
    			}
    		}
	    }
		return Msg.FAILED;
	}

}
