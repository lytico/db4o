/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
package com.db4o.cs.internal.config;

import com.db4o.config.*;
import com.db4o.cs.config.*;
import com.db4o.internal.*;
import com.db4o.internal.config.*;

public class Db4oClientServerLegacyConfigurationBridge {

	public static ClientConfiguration asClientConfiguration(Configuration config) {
		return new ClientConfigurationImpl((Config4Impl) config);
	}

	public static ServerConfiguration asServerConfiguration(Configuration config) {
		return new ServerConfigurationImpl((Config4Impl) config);
	}

	public static Config4Impl asLegacy(Object config) {
		return ((LegacyConfigurationProvider)config).legacy();
	}

	public static NetworkingConfiguration asNetworkingConfiguration(Configuration config) {
		return asServerConfiguration(config).networking();
	}

}
