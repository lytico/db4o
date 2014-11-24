/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.fileheader;

import com.db4o.ext.*;
import com.db4o.internal.*;


/**
 * @exclude
 */
public abstract class TimerFileLock implements Runnable{
    
	/**
	 * @sharpen.remove.first
	 */
    public static TimerFileLock forFile(LocalObjectContainer file){
    	
        if(file.needsLockFileThread()){
            return new TimerFileLockEnabled((IoAdaptedObjectContainer)file);
        }
        
        return new TimerFileLockDisabled();
    }

    public abstract void checkHeaderLock();

    public abstract void checkOpenTime();

    public abstract boolean lockFile();

    public abstract long openTime();

    public abstract void setAddresses(int baseAddress, int openTimeOffset, int accessTimeOffset);

    public abstract void start() throws Db4oIOException;

    public abstract void writeHeaderLock();

    public abstract void writeOpenTime();

    public abstract void close() throws Db4oIOException;

    public abstract void checkIfOtherSessionAlive(LocalObjectContainer container, int address,
		int offset, long lastAccessTime) throws Db4oIOException;
}
