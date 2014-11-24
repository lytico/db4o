/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.config.encoding;

/**
 * encodes a String to a byte array and decodes a String
 * from a part of a byte array  
 */
public interface StringEncoding {
	
	/**
	 * called when a string is to be encoded to a byte array.
	 * @param str the string to encode
	 * @return the encoded byte array
	 */
	public byte[] encode(String str);
	
	/**
	 * called when a byte array is to be decoded to a string.  
	 * @param bytes the byte array
	 * @param start the start offset in the byte array
	 * @param length the length of the encoded string in the byte array
	 * @return the string
	 */
	public String decode(byte[] bytes, int start, int length);

}
