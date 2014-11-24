/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.eventprocessor;

import com.db4o.drs.inside.*;
import com.db4o.drs.versant.*;
import com.db4o.drs.versant.ipc.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.util.*;


public class EventProcessorEmbedded implements Stoppable {
	
	private static final long STARTUP_TIMEOUT = DrsDebug.timeout(10000);

	private final Thread _eventProcessorThread;
	
	private final EventProcessorImpl _eventProcessor;

	public EventProcessorEmbedded(EventConfiguration eventConfiguration) {
		
		_eventProcessor = EventProcessorFactory.newInstance(eventConfiguration);
		_eventProcessorThread = new Thread(_eventProcessor, ReflectPlatform.simpleName(EventProcessorImpl.class)+" dedicated thread");
		_eventProcessorThread.setDaemon(true);
		_eventProcessorThread.start();
		
		final BlockingQueue4<String> barrier = new BlockingQueue<String>();
		_eventProcessor.addListener(new AbstractEventProcessorListener() {
			@Override
			public void ready() {
				barrier.add("ready");
			}
		});

		if (barrier.next(STARTUP_TIMEOUT) == null) {;
			throw new IllegalStateException(ReflectPlatform.simpleName(EventProcessorImpl.class) + " still not running after "+STARTUP_TIMEOUT+"ms");
		}
	}

	public void stop(){
		_eventProcessor.stop();
		try {
			_eventProcessorThread.join();
		} catch (InterruptedException e) {
			e.printStackTrace();
		}
	}

}
