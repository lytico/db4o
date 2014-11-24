package com.db4o.drs.versant.ipc.tcp;

import java.io.*;

public class ConnectionTimeoutException extends IOException {

	public ConnectionTimeoutException(String host, int port, long elapsed, int count, Throwable cause) {
		super("Giving up trying to connect to server ("+host+":"+port+") after " + count + " tries", cause);
	}

}
