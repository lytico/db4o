/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */
package com.db4o.cs.foundation;

import java.io.*;

public interface Socket4Factory {

	Socket4 createSocket(String hostName, int port) throws IOException;
	ServerSocket4 createServerSocket(int port) throws IOException;
}
