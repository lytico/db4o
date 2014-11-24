/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

class LongJumpOutException extends RuntimeException
{
	public Throwable fillInStackTrace() { return null;}
}
