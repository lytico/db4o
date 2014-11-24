/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.config;

import com.db4o.*;
import com.db4o.ext.*;

/**
 * @exclude
 */
public interface LegacyClientServerFactory {
	
	public ObjectContainer openClient(
			   Configuration config,
			   String hostName, 
			   int port, 
			   String user, 
			   String password)
			    throws 
			     Db4oIOException, 
			     OldFormatException,
			     InvalidPasswordException ;
			 
			 
			 public ObjectServer openServer(
			   Configuration config,
			   String databaseFileName, 
			   int port) 
			    throws 
			     Db4oIOException,
			     IncompatibleFileFormatException, 
			     OldFormatException,
			     DatabaseFileLockedException, 
			     DatabaseReadOnlyException;

}
