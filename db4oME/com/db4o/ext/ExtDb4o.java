/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.ext;

import com.db4o.*;

/**
 * extended factory class with static methods to open special db4o sessions.
 */
public final class ExtDb4o extends Db4o {
   /**
     * opens an {@link ObjectContainer} for in-memory use .
	 * <br><br>In-memory ObjectContainers are useful for maximum performance
	 * on small databases, for swapping objects or for storing db4o format data
	 * to other media or other databases.<br><br>Be aware of the danger of running
	 * into OutOfMemory problems or complete loss of all data, in case of hardware
	 * or JVM failures.<br><br>
     * @param memoryFile a {@link MemoryFile MemoryFile} 
     * to store the raw byte data.
	 * @return an open {@link com.db4o.ObjectContainer ObjectContainer}
     * @see MemoryFile
	 */
	public static final ObjectContainer openMemoryFile(MemoryFile memoryFile) {
		return openMemoryFile1(memoryFile);
	}
}
