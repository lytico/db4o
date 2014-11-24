/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.config;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.cs.config.*;
import com.db4o.cs.internal.*;
import com.db4o.ext.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public class LegacyClientServerFactoryImpl implements LegacyClientServerFactory{

	public ObjectContainer openClient(Configuration config, String hostName,
			int port, String user, String password) throws Db4oIOException,
			OldFormatException, InvalidPasswordException {
		if (user == null || password == null) {
			throw new InvalidPasswordException();
		}
		
		Config4Impl.assertIsNotTainted(config);
		ClientConfiguration clientConfig = Db4oClientServerLegacyConfigurationBridge.asClientConfiguration(config);
		Socket4Adapter networkSocket = new Socket4Adapter(clientConfig.networking().socketFactory(), hostName, port);
		return new ClientObjectContainer(clientConfig, networkSocket, user, password, true);
	}


	public ObjectServer openServer(Configuration config,
			String databaseFileName, int port)
			throws Db4oIOException, IncompatibleFileFormatException,
			OldFormatException, DatabaseFileLockedException,
			DatabaseReadOnlyException {
		LocalObjectContainer container = (LocalObjectContainer)Db4o.openFile(config, databaseFileName);
        if(container == null){
            return null;
        }
		ServerConfiguration serverConfig = Db4oClientServerLegacyConfigurationBridge.asServerConfiguration(config);
        synchronized(container.lock()){
            return new ObjectServerImpl(container, serverConfig, port);
        }
	}

}
