/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.foundation;

/**
 * @sharpen.ignore
 */
public class Lock4 {

    public <T> T run(Closure4<T> closure) {
    	synchronized(this){
    		return closure.run();
    	}
    }

    public void snooze(long timeout) {
    	try {
            wait(timeout);
        } catch (Exception e) {
        	e.printStackTrace();
        }
        
    }

    public void awake() {
    	notifyAll();
    }
}
