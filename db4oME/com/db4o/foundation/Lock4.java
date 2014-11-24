/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.foundation;

public class Lock4 {

    public Object run(Closure4 closure) throws Exception{
    	synchronized(this){
    		return closure.run();
    	}
    }

    public void snooze(long timeout) {
    	try {
            this.wait(timeout);
        } catch (Exception e) {
        }
        
    }

    public void awake() {
    	this.notify();
    }
}
