/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

/**
 * 
 */
package com.db4o.drs.test.versant;

import com.db4o.foundation.*;
import com.versant.event.*;

import db4ounit.*;

/**
* @exclude
*/
public class RecordingEventListener implements ClassEventListener {
	
	private Pair<Integer, VodEvent> _unexpectedEvent; 
	
	private final VodEvent[] _expectedEvents;
	
	private final Object _lock = new Object();
	
	private int _currentExpectedEvent;
	
	public RecordingEventListener(VodEvent... expectedEvents){
		_expectedEvents = expectedEvents;
		
	}
	
	private void eventHappened(VodEvent eventType, VersantEventObject event) {
		synchronized (_lock) {
			if(_currentExpectedEvent >= _expectedEvents.length){
				unexpectedEvent(_currentExpectedEvent, eventType);
				_lock.notifyAll();
				return;
			}
			if(_expectedEvents[_currentExpectedEvent] != eventType){
				unexpectedEvent(_currentExpectedEvent, eventType);
				_lock.notifyAll();
				return;
			}
			_currentExpectedEvent++;
			if(_currentExpectedEvent == _expectedEvents.length){
				_lock.notifyAll();
			}
		}
		
	}
	
	private void unexpectedEvent(int currentExpectedEvent, VodEvent eventType) {
		if(_unexpectedEvent != null){
			return;
		}
		_unexpectedEvent = new Pair(currentExpectedEvent, eventType);
	}

	public void instanceModified (VersantEventObject event){
		eventHappened(VodEvent.MODIFIED, event);
	}

	public void instanceCreated (VersantEventObject event) {
		eventHappened(VodEvent.CREATED, event);
	}

	public void instanceDeleted (VersantEventObject event) {
		eventHappened(VodEvent.DELETED, event);
	}
	
	public void verify(int timeOut){
		synchronized(_lock){
			if(_unexpectedEvent != null){
				Assert.fail("Unexpected event: " + _unexpectedEvent);
				return;
			}
			if(_currentExpectedEvent == _expectedEvents.length){
				return;
			}
			try {
				_lock.wait(timeOut);
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
			Assert.areEqual(_expectedEvents.length, _currentExpectedEvent);
		}
		
	}
	
}