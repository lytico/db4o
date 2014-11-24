/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

import com.db4o.*;


/**
 * @exclude
 */
public class BlockingQueueStoppedException extends RuntimeException {
	
	public BlockingQueueStoppedException(){
		super();
		if(DTrace.enabled){
			DTrace.BLOCKING_QUEUE_STOPPED_EXCEPTION.log();
		}
	}

}
