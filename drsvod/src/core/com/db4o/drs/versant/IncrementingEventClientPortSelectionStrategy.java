/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant;

public class IncrementingEventClientPortSelectionStrategy implements EventClientPortSelectionStrategy {

	private int _port;

	public IncrementingEventClientPortSelectionStrategy(int startPort) {
		_port = startPort;
	}
	
	public int clientPort() {
		return _port++;
	}

}
