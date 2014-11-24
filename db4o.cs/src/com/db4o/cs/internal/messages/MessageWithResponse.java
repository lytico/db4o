/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal.messages;

public interface MessageWithResponse {
	Msg replyFromServer();
	void postProcessAtServer();
}
