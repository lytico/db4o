/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.io;

/**
 * Representation of a container for storage of db4o
 * database data (to file, to memory). 
 */
public interface Bin {

	/**
	 * returns the length of the Bin (on disc, in memory).  
	 */
	long length();

	/**
	 * reads a given number of bytes into an array of bytes at an 
	 * offset position.
	 * @param position the offset position to read at
	 * @param bytes the byte array to read bytes into
	 * @param bytesToRead the number of bytes to be read
	 * @return The number of bytes actually read (<= bytesToRead) or -1 if position already
	 * points to/exceeds the end of the bin
	 */
	int read(long position, byte[] bytes, int bytesToRead);
	
	/**
	 * writes a given number of bytes from an array of bytes at 
	 * an offset position 
	 * @param position the offset position to write at
	 * @param bytes the array of bytes to write
	 * @param bytesToWrite the number of bytes to write
	 */
	void write(long position, byte[] bytes, int bytesToWrite);
	
	/**
	 * flushes the buffer content to the physical storage
	 * media.
	 */
	void sync();
	
	
	/**
	 * runs the Runnable between two calls to sync();
	 */
	void sync(Runnable runnable);
	
	
	/**
	 * reads a given number of bytes into an array of bytes at an 
	 * offset position. In contrast to the normal {@link #read(long, byte[], int)}
	 * method, the Bin should ensure direct access to the raw storage medium.
	 * No caching should take place.
	 * @param position the offset position to read at
	 * @param bytes the byte array to read bytes into
	 * @param bytesToRead the number of bytes to be read
	 * @return
	 */
	int syncRead(long position, byte[] bytes, int bytesToRead);


	/**
	 * closes the Bin.
	 */
	void close();
	
	

}