/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.ext;

/**
 * carries in-memory data for db4o in-memory operation.
 * <br><br>In-memory ObjectContainers are useful for maximum performance
 * on small databases, for swapping objects or for storing db4o format data
 * to other media or other databases.<br><br>Be aware of the danger of running
 * into OutOfMemory problems or complete loss of all data, in case of hardware
 * or JVM failures.
 * <br><br>
 * @see ExtDb4o#openMemoryFile
 */
public class MemoryFile {

	private byte[] i_bytes;
	private final int INITIAL_SIZE_AND_INC = 10000;
	private int i_initialSize = INITIAL_SIZE_AND_INC;
	private int i_incrementSizeBy = INITIAL_SIZE_AND_INC;
	

	/**
	 * constructs a new MemoryFile without any data.
	 * @see ExtDb4o#openMemoryFile
	 */
	public MemoryFile() {
	}

	/**
	 * constructs a MemoryFile to use the byte data from a previous
	 * MemoryFile.
	 * @param bytes the raw byte data.
	 * @see ExtDb4o#openMemoryFile
	 */
	public MemoryFile(byte[] bytes) {
		i_bytes = bytes;
	}
	
	/**
	 * returns the raw byte data.
	 * <br><br>Use this method to get the byte data from the MemoryFile
	 * to store it to other media or databases, for backup purposes or
	 * to create other MemoryFile sessions.
	 * <br><br>The byte data from a MemoryFile should only be used
	 * after it is closed.<br><br>
	 * @return bytes the raw byte data.
	 */
	public byte[] getBytes(){
		if(i_bytes == null){
			return new byte[0];
		}
		return i_bytes;
	}
	
	/**
	 * returns the size the MemoryFile is to be enlarged, if it grows beyond
	 * the current size.
	 * @return size in bytes
	 */
	public int getIncrementSizeBy(){
		return i_incrementSizeBy;
	}
	
	/**
	 * returns the initial size of the MemoryFile.
	 * @return size in bytes
	 */
	public int getInitialSize(){
		return i_initialSize;
	}
	
	/**
	 * sets the raw byte data.
	 * <br><br><b>Caution!</b><br>Calling this method during a running
	 * Memory File session may produce unpreditable results.
	 * @param bytes the raw byte data.
	 */
	public void setBytes(byte[] bytes){
		i_bytes = bytes;
	}

	/**
	 * configures the size the MemoryFile is to be enlarged by, if it grows
	 * beyond the current size.
	 * <br><br>Call this method before passing the MemoryFile to 
	 * {@link com.db4o.ext.ExtDb4o#openMemoryFile ExtDb4o#openMemoryFile(MemoryFile)}.
	 * <br><br>
	 * This parameter can be modified to tune the maximum performance of
	 * a MemoryFile for a specific usecase. To produce the best results,
	 * test the speed of your application with real data.<br><br>
	 * @param byteCount the desired size in bytes
	 */
	public void setIncrementSizeBy(int byteCount){
		i_incrementSizeBy = byteCount;
	}
	
	/**
	 * configures the initial size of the MemoryFile.
	 * <br><br>Call this method before passing the MemoryFile to 
	 * {@link com.db4o.ext.ExtDb4o#openMemoryFile ExtDb4o#openMemoryFile(MemoryFile)}.
	 * <br><br>
	 * This parameter can be modified to tune the maximum performance of
	 * a MemoryFile for a specific usecase. To produce the best results,
	 * test speed and memory consumption of your application with
	 * real data.<br><br>
	 * @param byteCount the desired size in bytes
	 */
	public void setInitialSize(int byteCount){
		i_initialSize = byteCount;
	}
}