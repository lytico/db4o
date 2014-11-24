/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;

class ShutDownRunnable extends Collection4 implements Runnable {
	
	public volatile boolean dontRemove = false;
	
	public void run(){
		dontRemove = true;
		Iterator4 i = iterator();
		while(i.hasNext()){
			((YapStream)i.next()).failedToShutDown();
		}
	}
	
}

