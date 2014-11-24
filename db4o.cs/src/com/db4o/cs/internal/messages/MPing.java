/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.messages;



/**
 * @exclude
 */
public class MPing extends Msg implements MessageWithResponse {

	public Msg replyFromServer() {
	    return Msg.PONG;
	}
	
}
