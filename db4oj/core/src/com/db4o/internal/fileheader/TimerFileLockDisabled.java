/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.fileheader;

import com.db4o.ext.*;
import com.db4o.internal.*;


/**
 * @exclude
 */
public class TimerFileLockDisabled  extends TimerFileLock{
    
    public void checkHeaderLock() {
    }

    public void checkOpenTime() {
    }

    public void close() {
    }
    
    public boolean lockFile() {
        return false;
    }

    public long openTime() {
        return 0;
    }

    public void run() {
    }

    public void setAddresses(int baseAddress, int openTimeOffset, int accessTimeOffset) {
    }

    public void start() {
    }

    public void writeHeaderLock(){
    }

    public void writeOpenTime() {
    }

	public void checkIfOtherSessionAlive(LocalObjectContainer container, int address, int offset,
		long lastAccessTime) throws Db4oIOException {		
	}


    
}
