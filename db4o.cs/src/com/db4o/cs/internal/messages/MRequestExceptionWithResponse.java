/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

import com.db4o.internal.*;


public class MRequestExceptionWithResponse extends MsgD implements MessageWithResponse {
	
	public Msg replyFromServer() {
		Platform4.throwUncheckedException((Throwable) readSingleObject());
		return null;
	}

}
