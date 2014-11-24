/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

/**
 * TODO: Do we need this class? Possibly it's initialized by reflection
 * during a license check to bypass hacks. 
 */
class UserException extends RuntimeException
{
	final int errCode;
	final String errMsg;
	
	UserException(int a_code, String a_msg, int a){
		errCode = a_code;
		errMsg = a_msg;
	}
}
