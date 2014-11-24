/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.encoding;

import java.io.*;

import com.db4o.ext.*;

/**
 * @exclude
 * @sharpen.ignore
 */
public class UTF8StringEncoding extends BuiltInStringEncoding{
	
	private final static String CHARSET_NAME = "UTF-8";
	
	public byte[] encode(String str) {
		try {
			return str.getBytes(CHARSET_NAME);
		} catch (UnsupportedEncodingException e) {
			throw new Db4oIOException(e);
		}
	}
	
	public String decode(byte[] bytes, int start, int length) {
		try {
			return new String(bytes, start, length, CHARSET_NAME);
		} catch (UnsupportedEncodingException e) {
			throw new Db4oIOException(e);
		}
	}

}
