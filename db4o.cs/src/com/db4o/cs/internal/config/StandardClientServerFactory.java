/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.cs.internal.config;

import com.db4o.*;
import com.db4o.cs.config.*;
import com.db4o.cs.internal.*;
import com.db4o.ext.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public class StandardClientServerFactory implements ClientServerFactory{

	public ObjectContainer openClient(ClientConfiguration clientConfig, String hostName,
			int port, String user, String password) throws Db4oIOException,
			OldFormatException, InvalidPasswordException {
		if (user == null || password == null) {
			throw new InvalidPasswordException();
		}
		
		Config4Impl config = asLegacy(clientConfig);
		Config4Impl.assertIsNotTainted(config);
		Socket4Adapter networkSocket = new Socket4Adapter(clientConfig.networking().socketFactory(), hostName, port);
		return new ClientObjectContainer(clientConfig, networkSocket, user, password, true);
	}


	public ObjectServer openServer(ServerConfiguration config,
			String databaseFileName, int port)
			throws Db4oIOException, IncompatibleFileFormatException,
			OldFormatException, DatabaseFileLockedException,
			DatabaseReadOnlyException {
		LocalObjectContainer container = (LocalObjectContainer)Db4o.openFile(asLegacy(config), databaseFileName);
        if(container == null){
            return null;
        }
        synchronized(container.lock()){
            return new ObjectServerImpl(container, config, port);
        }
	}

	private Config4Impl asLegacy(Object config) {
		return Db4oClientServerLegacyConfigurationBridge.asLegacy(config);
	}
}
