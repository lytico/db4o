/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.foundation;

import com.db4o.drs.inside.*;


/**
 *  Experiment field for future db4o logging.
 *  This will become an interface in the future.
 *  It will also allow wrapping to Log4j
 *  For now we are just collecting requirments.
 *  
 *  We are not using log4j directly on purpose, so
 *  we can keep the footprint small for embedded
 *  devices
 */
public class Logger4 {
	
	public void logIdentity(Object obj, String message) {
		if(obj == null){
			log(message + " [null]");
		}
		log(message + " "  + obj.getClass() + " " + System.identityHashCode(obj));
	}
	
	public void log(String message){
		if(! DrsDebug.verbose){
			return;
		}
		System.out.println(StackAnalyzer.methodCallAboveAsString(Logger4Support.class) + " " + message);
		System.out.flush();
	}

}
