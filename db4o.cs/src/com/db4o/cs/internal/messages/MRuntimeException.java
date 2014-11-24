/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */
package com.db4o.cs.internal.messages;

public class MRuntimeException extends MsgD {

	public void throwPayload() {
		throw ((RuntimeException)readSingleObject());
	}
}
