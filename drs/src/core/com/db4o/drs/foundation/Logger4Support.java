/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.foundation;


/**
 * NOTE: Sharpen required to add a -fullyQualify Logger4Support
 * to the sharpen-dRS-options in db4obuild.
 * If we reuse Logger4Support in db4o, we have to add the
 * same entry to sharpen-all-options
 */


/**
 *  Experiment field for future db4o logging.
 *  This will become an interface in the future.
 *  It will also allow wrapping to Log4j
 *  For now we are just collecting requirments.
 *  
 *  We are not using log4j directly on purpose, so
 *  we can keep the footprint small for embedded
 *  devices
 *  
 *  
 */
public class Logger4Support {
	
	private static final Logger4 _logger = new Logger4();
	
	public static void logIdentity(Object obj, String message){
		_logger.logIdentity(obj, message);
	}
	
	public static void logIdentity(Object obj) {
		logIdentity(obj, "");
	}
	
	public static void log(String str){
		_logger.log(str);
	}
	
	

}
