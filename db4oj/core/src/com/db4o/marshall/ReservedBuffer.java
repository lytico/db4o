/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.marshall;


/**
 * a reserved buffer within a write buffer.
 * The usecase this class was written for: A null bitmap should be at the 
 * beginning of a slot to allow lazy processing. During writing the content 
 * of the null bitmap is not yet fully known until all members are processed.
 * With the Reservedbuffer the space in the slot can be occupied and writing
 * can happen after all members are processed. 
 */
public interface ReservedBuffer {

    /**
     * writes a byte array to the reserved buffer.
     * @param bytes the byte array.
     */
    public void writeBytes(byte[] bytes);
    
}
