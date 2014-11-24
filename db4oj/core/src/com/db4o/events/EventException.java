package com.db4o.events;

import com.db4o.ext.*;

/**
 * db4o-specific exception.<br><br>
 * Exception thrown during event dispatching if a client
 * provided event handler throws.<br/><br/>
 * 
 * The exception threw by the client can be retrieved by 
 * calling EventException#getCause().
 */
public class EventException extends Db4oRecoverableException {
	public EventException(final Throwable exc) {
		super(exc);
	}
}
