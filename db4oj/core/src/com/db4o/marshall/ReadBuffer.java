/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.marshall;

import com.db4o.foundation.*;

/**
 * a buffer interface with methods to read and to position 
 * the read pointer in the buffer.
 */
public interface ReadBuffer {
    
	/**
	 * returns the current offset in the buffer
	 * @return the offset
	 */
    int offset();
    
    public BitMap4 readBitMap(int bitCount);

    /**
     * reads a byte from the buffer.
     * @return the byte
     */
    byte readByte();
    
    /**
     * reads an array of bytes from the buffer.
     * The length of the array that is passed as a parameter specifies the
     * number of bytes that are to be read. The passed bytes buffer parameter
     * is directly filled.  
     * @param bytes the byte array to read the bytes into.
     */
    void readBytes(byte[] bytes);

    /**
     * reads an int from the buffer.
     * @return the int
     */
    int readInt();
    
    /**
     * reads a long from the buffer.
     * @return the long
     */
    long readLong();
    
    /**
     * positions the read pointer at the specified position
     * @param offset the desired position in the buffer
     */
	void seek(int offset);
}
