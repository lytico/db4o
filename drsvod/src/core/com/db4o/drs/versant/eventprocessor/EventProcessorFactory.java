/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.eventprocessor;

import com.db4o.drs.versant.*;
import com.versant.event.*;

public class EventProcessorFactory {
	
	public static EventProcessorImpl newInstance (EventConfiguration eventConfiguration) {
		VodEventClient client = new VodEventClient(eventConfiguration, new ExceptionListener (){
	        public void exceptionOccurred (Throwable exception){
	        	EventProcessorImpl.unrecoverableExceptionOccurred(exception);
	        }
	    });
		VodDatabase vod = new VodDatabase(eventConfiguration);
		EventProcessorImpl eventProcessor = new EventProcessorImpl(client, vod, eventConfiguration.verbose);
		return eventProcessor;
	}

}
