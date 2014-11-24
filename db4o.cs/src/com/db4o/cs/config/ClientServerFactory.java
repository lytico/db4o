/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.config;

import com.db4o.*;
import com.db4o.ext.*;

/**
 * factory to open C/S server and client implementations.
 * @see com.db4o.cs.Db4oClientServer#openClient(ClientConfiguration, String, int, String, String)
 * @see com.db4o.cs.Db4oClientServer#openServer(ServerConfiguration, String, int)
 */
public interface ClientServerFactory {
	
	public ObjectContainer openClient(
			ClientConfiguration config,
			String hostName, 
			int port, 
			String user, 
			String password)
				throws 
					Db4oIOException, 
					OldFormatException,
					InvalidPasswordException ;
	
	
	public ObjectServer openServer(
			ServerConfiguration config,
			String databaseFileName, 
			int port) 
				throws 
					Db4oIOException,
					IncompatibleFileFormatException, 
					OldFormatException,
					DatabaseFileLockedException, 
					DatabaseReadOnlyException;

}
