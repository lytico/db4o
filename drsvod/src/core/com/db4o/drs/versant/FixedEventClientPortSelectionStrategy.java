/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant;

public class FixedEventClientPortSelectionStrategy implements EventClientPortSelectionStrategy {

	private final int _port;

	public FixedEventClientPortSelectionStrategy(int port) {
		_port = port;
	}
	
	public int clientPort() {
		return _port;
	}

}
