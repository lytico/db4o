/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */

package com.db4o.ext;

/**
 * db4o-specific exception.<br><br>
 * This exception is thrown when db4o reads slot
 * information which is not valid (length or address).
 */
public class InvalidSlotException extends Db4oRecoverableException {

	/**
	 * Constructor allowing to specify a detailed message.
	 * @param msg message
	 */
	public InvalidSlotException(String msg) {
		super(msg);
	}
	
	/**
	 * Constructor allowing to specify the address, length and id.
	 * @param address offending address
	 * @param length offending length
	 * @param id id where the address and length were read. 
	 */
	public InvalidSlotException(int address, int length, int id) {
		super("address: " + address + ", length : " + length + ", id : " + id);
	}

}
