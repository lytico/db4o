/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

package com.db4o.cs.internal;

import com.db4o.ext.*;

/**
 * This class exists to work around a decaf conversion problem
 * when the code was directly in ServerMessageDispatcherImp.
 *  
 * @exclude
 */
class FatalServerShutdown {

	FatalServerShutdown(ObjectServerImpl server, Throwable origExc) {
		try {
			server.close(ShutdownMode.fatal(origExc));
		}
		catch(Throwable throwable) {
			throw new CompositeDb4oException(origExc, throwable);
		}
	}

}
