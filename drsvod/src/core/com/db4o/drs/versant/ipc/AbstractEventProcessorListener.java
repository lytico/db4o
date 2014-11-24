/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.ipc;

import com.db4o.drs.versant.ipc.EventProcessor.*;

public class AbstractEventProcessorListener implements EventProcessorListener{

	public void committed(String transactionId) {
		// do nothing
	}

	public void onEvent(long loid, long version) {
		// do nothing
	}

	public void ready() {
		// do nothing
	}

}
