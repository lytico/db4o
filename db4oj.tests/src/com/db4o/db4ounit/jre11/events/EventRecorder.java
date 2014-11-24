/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11.events;

import java.util.*;

import com.db4o.events.*;

public class EventRecorder implements EventListener4 {
	
	private final Object _lock;
	
	private static final long TIMEOUT = 10000; // 10 seconds
	
	private final Vector _events = new Vector();
	
	private boolean _cancel;
	
	public EventRecorder(Object lock_){
		_lock = lock_;
	}
	
	public String toString() {
		return _events.toString();
	}
	
	public void onEvent(Event4 e, EventArgs args) {
		synchronized(_lock){
			if (_cancel && args instanceof CancellableEventArgs) {
				((CancellableEventArgs)args).cancel();
			}
			_events.addElement(new EventRecord(e, args));
			_lock.notifyAll();
		}
	}

	public int size() {
		synchronized(_lock){
			return _events.size();
		}
	}

	public EventRecord get(int index) {
		return (EventRecord)_events.elementAt(index);
	}

	public void clear() {
        _events.removeAllElements();
	}

	public void cancel(boolean flag) {
		_cancel = flag;
	}
	
	public void waitForEventCount(int count){
		synchronized(_lock){
			long startTime = System.currentTimeMillis();
			while(size() < count){
				try {
					_lock.wait();
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
				long currentTime = System.currentTimeMillis();
				long duration = currentTime - startTime;
				if(duration > TIMEOUT){
					throw new RuntimeException("EventRecorder timed out waiting for " + count + " events to happen.");
				}
			}
		}
	}
	
}