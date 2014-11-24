/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.marshall;


/**
 * a buffer interface with write methods.
 */
public interface WriteBuffer {

    /**
     * writes a single byte to the buffer.
     * @param b the byte
     */
    void writeByte(byte b);
    
    /**
     * writes an array of bytes to the buffer
     * @param bytes the byte array
     */
    void writeBytes(byte[] bytes);

    /**
     * writes an int to the buffer.
     * @param i the int
     */
    void writeInt(int i);
    
    /**
     * writes a long to the buffer
     * @param l the long
     */
    void writeLong(long l);
    
}
